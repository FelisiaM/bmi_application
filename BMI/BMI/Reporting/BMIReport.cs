﻿using System;
using System.Collections.Generic;
using System.Linq;
using BMI.Models;

namespace BMI.Reporting
{
    public class BmiReport : IBmiReport
    {
        public double GetBmiIndex(double height, double weight)
        {
            if (height == 0)
            {
                throw new ArgumentException("height");
            }

            if (weight == 0)
            {
                throw new ArgumentException("weight");
            }

            var index = weight / (height * height);

            return Math.Round(index, 1, MidpointRounding.AwayFromZero);
        }

        public string GetBmiCategory(double index)
        {
            if (index == 0)
            {
                throw new ArgumentException("weight");
            }

            if (index <= 18.5)
            {
                return BmiCategory.Underweight;
            }
            if (index >= 18.6 && index <= 24.9)
            {
                return BmiCategory.Normal;
            }
            if (index >= 25 && index <= 29.9)
            {
                return BmiCategory.Preobesity;
            }
            if (index >= 30 && index <= 34.9)
            {
                return BmiCategory.ObesityClass1;
            }
            return BmiCategory.Undefined;
        }

        public Dictionary<string, int> GetBmiPopulationReport(
            List<UserDetails> measurmentsList)
        {
            return GetBmiIndexForAllEntries(measurmentsList)
                .Select(GetBmiCategory)
                .GroupBy(o => o)
                .ToDictionary(category => category.Key, category => category.Count());
        }

        public double GetUsersPercentile(List<UserDetails> measurmentsList, double userToFind)
        {
            var allEntries = GetBmiIndexForAllEntries(measurmentsList);
            var list = allEntries.Append(userToFind).ToList();

            list.Sort();
            var indexInList = list.BinarySearch(userToFind);

            var rank =  indexInList / (double)list.Count * 100;

            return Math.Round(rank, MidpointRounding.AwayFromZero);
        }

        private IEnumerable<double> GetBmiIndexForAllEntries(IEnumerable<UserDetails> measurmentsList)
        {
            return measurmentsList
                .Select(o => GetBmiIndex(o.Height, o.Weight));
        }
    }
}
