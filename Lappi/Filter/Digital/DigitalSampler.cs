﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Lappi.Filter.Digital {

    public class DigitalSampler : DigitalSampler<double> {

        public DigitalSampler (DigitalFilter filter) : base(filter) {
        }

    }

    public class DigitalSampler<T> where T : new() {

        private readonly DigitalFilter filter;
        private readonly double[] coefficients;
        private readonly IntegralArray<double> sums;
        private readonly IntegralArray<double> altSums;

        public DigitalSampler (DigitalFilter filter) {
            this.filter = filter;
            coefficients = filter.Coefficients;
            sums = new IntegralArray<double>(coefficients);
            altSums = new IntegralArray<double>(coefficients.Select((x, i) => i % 2 == 0 ? x : -x).ToArray());
        }

        public T Sample (T[] source, int center) {
            int shift = center + filter.Left;
            int left = Math.Max(0, -shift);
            int right = Math.Min(coefficients.Length - 1, source.Length - 1 - shift);
            T sum = new T();
            for( int i = left; i <= right; i++ ) {
                sum += (dynamic) source[i + shift] * coefficients[i];
            }
            sum /= (dynamic) Normalize(left, right);
            return sum;
        }

        public T SampleHighpass (T[] source, int center) {
            return (dynamic) source[center] - Sample(source, center);
        }

        public T[] Convolute (T[] source) {
            T[] result = new T[source.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = Sample(source, i);
            }
            return result;
        }

        public T[] ConvoluteHighpass (T[] source) {
            T[] result = new T[source.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = SampleHighpass(source, i);
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

        public T[] DownsampleHighpass (T[] source, int factor, int shift) {
            T[] result = new T[source.Length / factor];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = SampleHighpass(source, i * factor + shift);
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

        public T[] UpsampleHighpass (T[] source, int factor, int shift) {
            T[] v = new T[source.Length * factor];
            if( default(T) == null ) {
                for( int i = 0; i < v.Length; i++ ) {
                    v[i] = new T();
                }
            }
            for( int i = 0; i < source.Length; i++ ) {
                v[i * factor + shift] = source[i];
            }
            return ConvoluteHighpass(v);
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private double Normalize (int left, int right) {
            double sum = Math.Abs(sums.Sum(left, right));
            double altSum = Math.Abs(altSums.Sum(left, right));
            double factor = Math.Max(Math.Abs(sum), Math.Abs(altSum));
            return factor == 0 ? 1 : factor;
        }

    }

}
