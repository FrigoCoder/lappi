﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Lappi.Filter.Digital {

    public class DigitalSampler : DigitalSampler<double> {

        public DigitalSampler (DigitalFilter filter) : base(filter) {
        }

    }

    public class DigitalSampler<T> where T : new() {

        private readonly DigitalFilter filter;

        public DigitalSampler (DigitalFilter filter) {
            this.filter = filter;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public T Sample (T[] source, int center) {
            T result = new T();
            double sum = 0;
            int left = Math.Max(center + filter.Left, 0);
            int right = Math.Min(center + filter.Right, source.Length - 1);
            Func<int, double> kernel = filter.Kernel;
            for( int index = left; index <= right; index++ ) {
                double weight = kernel(index - center);
                result += (dynamic) source[index] * weight;
                sum += Math.Abs(weight);
            }
            if( sum != 0 ) {
                result /= (dynamic) sum;
            }
            return result;
        }

        public T[] Convolute (T[] source) {
            T[] result = new T[source.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = Sample(source, i);
            }
            return result;
        }

        public T[] Downsample (T[] source, int factor, int shift) {
            T[] result = new T[source.Length / factor];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = Sample(source, i * factor + shift);
            }
            return result;
        }

        public T[] Upsample (T[] source, int factor, int shift) {
            T[] v = new T[source.Length * factor];
            if( default(T) == null ) {
                for( int i = 0; i < v.Length; i++ ) {
                    v[i] = new T();
                }
            }
            for( int i = 0; i < source.Length; i++ ) {
                v[i * factor + shift] = source[i];
            }
            return Convolute(v);
        }

    }

}
