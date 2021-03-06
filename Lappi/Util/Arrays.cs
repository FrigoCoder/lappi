﻿using System;

namespace Lappi.Util {

    public static class Arrays {

        public static T[] New<T> (int length, T defaultValue) {
            T[] v = new T[length];
            for( int i = 0; i < length; i++ ) {
                v[i] = defaultValue;
            }
            return v;
        }

        public static T[] New<T> (int length, Func<int, T> f) {
            T[] v = new T[length];
            for( int i = 0; i < length; i++ ) {
                v[i] = f(i);
            }
            return v;
        }

        public static T[] New<T> (int length, Func<int, T[], T> f) {
            T[] v = new T[length];
            for( int i = 0; i < length; i++ ) {
                v[i] = f(i, v);
            }
            return v;
        }

        public static T[,] New<T> (int length1, int length2, T defaultValue) {
            T[,] v = new T[length1, length2];
            for( int i = 0; i < length1; i++ ) {
                for( int j = 0; j < length2; j++ ) {
                    v[i, j] = defaultValue;
                }
            }
            return v;
        }

        public static T[,] New<T> (int length1, int length2, Func<int, int, T> f) {
            T[,] v = new T[length1, length2];
            for( int i = 0; i < length1; i++ ) {
                for( int j = 0; j < length2; j++ ) {
                    v[i, j] = f(i, j);
                }
            }
            return v;
        }

        public static T[,] New<T> (int length1, int length2, Func<int, int, T[,], T> f) {
            T[,] v = new T[length1, length2];
            for( int i = 0; i < length1; i++ ) {
                for( int j = 0; j < length2; j++ ) {
                    v[i, j] = f(i, j, v);
                }
            }
            return v;
        }

        public static T[] Add<T> (this T[] u, T[] v) {
            Preconditions.Require(u.Length == v.Length);
            return New<T>(u.Length, i => (dynamic) u[i] + v[i]);
        }

        public static T[] Sub<T> (this T[] u, T[] v) {
            Preconditions.Require(u.Length == v.Length);
            return New<T>(u.Length, i => (dynamic) u[i] - v[i]);
        }

        public static void Foreach<T> (this T[] v, Action<T> action) {
            foreach( T item in v ) {
                action(item);
            }
        }

        public static void Foreach<T> (this T[] v, Action<T, int> action) {
            for( int i = 0; i < v.Length; i++ ) {
                action(v[i], i);
            }
        }

        public static void Foreach<T> (this T[] v, Action<T, int, T[]> action) {
            for( int i = 0; i < v.Length; i++ ) {
                action(v[i], i, v);
            }
        }

        public static void Foreach<T> (this T[,] v, Action<T> action) {
            foreach( T item in v ) {
                action(item);
            }
        }

        public static void Foreach<T> (this T[,] v, Action<T, int, int> action) {
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    action(v[i, j], i, j);
                }
            }
        }

        public static void Foreach<T> (this T[,] v, Action<T, int, int, T[,]> action) {
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    action(v[i, j], i, j, v);
                }
            }
        }

    }

}
