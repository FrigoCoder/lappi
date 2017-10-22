﻿using System;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class Laplacian1DTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};
        private readonly DigitalFilter analysis = new DigitalAdapter(new Linear(), 2.0);
        private readonly DigitalFilter synthesis = new DigitalAdapter(new Linear(), 2.0);
        private Laplacian1D laplacian;

        [SetUp]
        public void SetUp () {
            laplacian = new Laplacian1D(analysis, synthesis);
        }

        [TestCase]
        public void Downsample_works_correctly () {
            double[] expected = {2, 9.5, 25.5};

            Tuple<double[], double[]> result = laplacian.Transform(source);
            Assert.That(result.Item1, Is.EqualTo(expected));
        }

    }

}