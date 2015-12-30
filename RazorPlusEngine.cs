using RazorEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace RMA.Classes
{
    public static class RazorPlusEngine
    {

        private const string _firstHeader = "@using RMA; @{var n1 = 1; var n2 = 1; var n3 = 1; var n4 = 1; var n5 = 1; var n6 = 1; var n7 = 1;"
                            + " var n8 = 1; var n9 = 1; var n10 = 1; var n11 = 1; var n12 = 1; var n13 = 1; var n14 = 1; var n15 = 1;";

        private const string _itemToHidePattern = "@hide\\[\"(.*?)\"]";
        
        private const string _r_itemToHidePattern = "@repeat\\[\"hide_(.*?)\"]";

        public static string razorPlusParser(SerializableDictionary<string, string> projectItemDic,
                                             SerializableDictionary<string, Collection<SerializableDictionary<string, string>>> projectRptDic,
                                             string template,
                                             string templateFromGenerator
                                             )
        {
            string headTemplate;
            string result = null;

            if ((projectItemDic != null && projectItemDic.Count > 0) && (projectRptDic != null && projectRptDic.Count > 0))
            {
                List<List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>> razorModel =
                    new List<List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>>();

                razorModel.Add(new List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>());
                razorModel[0].Add(new List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>());
                razorModel[0][0].Add(new SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>());
                razorModel[0][0][0].Add("item", new Collection<SerializableDictionary<string, string>>());
                razorModel[0][0][0]["item"].Add(new SerializableDictionary<string, string>());
                razorModel[0][0][0]["item"][0] = projectItemDic;

                razorModel.Add(new List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>());
                razorModel[1].Add(new List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>());
                razorModel[1][0].Add(projectRptDic);

                headTemplate = _firstHeader
                             + "var rpt = Model[1][0][0];"
                             + "var TEXT = Model[0][0][0][\"item\"][0];"
                             + "var text = Model[0][0][0][\"item\"][0];"
                             + "var DATE = Model[0][0][0][\"item\"][0];"
                             + "var date = Model[0][0][0][\"item\"][0];"
                             + "var LIST = Model[0][0][0][\"item\"][0];"
                             + "var list = Model[0][0][0][\"item\"][0]; }";
                            /*+ "PROJECT"
                              + "project"
                              + "DIRECTORY"
                              + "directory"
                              + "HIDE"
                              + "hide"
                              + "PATTERN"
                              + "pattern";*/

                string bodyTemplate = null;

                //////////////////////

                if (template != null) bodyTemplate = Regex.Replace(template, _itemToHidePattern, "");

                else if (templateFromGenerator != null) bodyTemplate = Regex.Replace(templateFromGenerator, _itemToHidePattern, "");

                //////////////////////

                if (bodyTemplate != null) bodyTemplate = Regex.Replace(bodyTemplate, _r_itemToHidePattern, "");

                //////////////////////

                string razorTemplate = headTemplate + Environment.NewLine + Environment.NewLine + bodyTemplate;

                ///////////////////
                // Parsing Start //
                ///////////////////

                result = Razor.Parse(razorTemplate, razorModel);

                ///////////////////
                // Parsing End //
                ///////////////////
            }

            else if (projectItemDic != null && projectItemDic.Count > 0)
            {
                headTemplate = _firstHeader
                             + "var TEXT = Model;"
                             + "var text = Model;"
                             + "var DATE = Model;"
                             + "var date = Model;"
                             + "var LIST = Model;"
                             + "var list = Model; }";

                string bodyTemplate = "";

                if (template != "") bodyTemplate = Regex.Replace(template, _itemToHidePattern, "");

                else if (templateFromGenerator != "") bodyTemplate = Regex.Replace(templateFromGenerator, _itemToHidePattern, "");

                if (bodyTemplate != "") bodyTemplate = Regex.Replace(bodyTemplate, _r_itemToHidePattern, "");

                string razorTemplate = headTemplate + Environment.NewLine + Environment.NewLine + bodyTemplate;

                ///////////////////
                // Parsing Start //
                ///////////////////

                result = Razor.Parse(razorTemplate, projectItemDic);

                /////////////////
                // Parsing End //
                /////////////////
            }

            else if (projectRptDic != null && projectRptDic.Count > 0)
            {
                headTemplate = _firstHeader + "var rpt = Model;}";

                string bodyTemplate = "";

                if (template != "") bodyTemplate = Regex.Replace(template, _itemToHidePattern, "");

                else if (templateFromGenerator != "") bodyTemplate = Regex.Replace(templateFromGenerator, _itemToHidePattern, "");

                if (bodyTemplate != "") bodyTemplate = Regex.Replace(bodyTemplate, _r_itemToHidePattern, "");

                string razorTemplate = headTemplate + Environment.NewLine + Environment.NewLine + bodyTemplate;

                ///////////////////
                // Parsing Start //
                ///////////////////

                result = Razor.Parse(razorTemplate, projectRptDic);

                /////////////////
                // Parsing End //
                /////////////////
            }

            return result;
        }
    }
}
