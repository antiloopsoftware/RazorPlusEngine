using RazorEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace OBAG.RazorPlusEngine
{
    public static class Parsing
    {
        private const string _firstHeader = "@using OBAG; "
                                          + "@{var n1 = 1; var n2 = 1; var n3 = 1; var n4 = 1; var n5 = 1; var n6 = 1; var n7 = 1;"
                                          + " var n8 = 1; var n9 = 1; var n10 = 1; var n11 = 1; var n12 = 1; var n13 = 1; var n14 = 1; var n15 = 1;";

        private const string _itemToHidePattern = "@hide\\[\"(.*?)\"]";
        
        private const string _r_itemToHidePattern = "@repeat\\[\"hide_(.*?)\"]";

        public static string razorPlusParser(SerializableDictionary<string, string> projectStaticItemDic,
                                             SerializableDictionary<string, Collection<SerializableDictionary<string, string>>> projectRepeaterDic,
                                             string template
                                             )
        {
            string headTemplate;
            string result = null;

            if ((projectStaticItemDic != null && projectStaticItemDic.Count > 0) && (projectRepeaterDic != null && projectRepeaterDic.Count > 0))
            {
                List<List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>> razorModel =
                    new List<List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>>();

                razorModel.Add(new List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>());
                razorModel[0].Add(new List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>());
                razorModel[0][0].Add(new SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>());
                razorModel[0][0][0].Add("item", new Collection<SerializableDictionary<string, string>>());
                razorModel[0][0][0]["item"].Add(new SerializableDictionary<string, string>());
                razorModel[0][0][0]["item"][0] = projectStaticItemDic;

                razorModel.Add(new List<List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>>());
                razorModel[1].Add(new List<SerializableDictionary<string, Collection<SerializableDictionary<string, string>>>>());
                razorModel[1][0].Add(projectRepeaterDic);

                headTemplate = _firstHeader
                             
                             + "var repeater = Model[1][0][0];"
                             
                             + "var TEXT = Model[0][0][0][\"item\"][0];"
                             + "var text = Model[0][0][0][\"item\"][0];"
                             
                             + "var NUMBER = Model[0][0][0][\"item\"][0];"
                             + "var number = Model[0][0][0][\"item\"][0];"
                             
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

                if (!String.IsNullOrWhiteSpace(template))
                    bodyTemplate = Regex.Replace(template, _itemToHidePattern, String.Empty);

                //////////////////////

                if (!String.IsNullOrWhiteSpace(bodyTemplate))
                    bodyTemplate = Regex.Replace(bodyTemplate, _r_itemToHidePattern, String.Empty);

                //////////////////////

                string razorTemplate = headTemplate + Environment.NewLine + Environment.NewLine 
                                     + bodyTemplate;

                ///////////////////
                // Parsing Start //
                ///////////////////

                result = Razor.Parse(razorTemplate, razorModel);

                ///////////////////
                // Parsing End //
                ///////////////////
            }

            else if (projectStaticItemDic != null && projectStaticItemDic.Count > 0)
            {
                headTemplate = _firstHeader
                             
                             + "var TEXT = Model;"
                             + "var text = Model;"
                             
                             + "var NUMBER = Model;"
                             + "var number = Model;"

                             + "var DATE = Model;"
                             + "var date = Model;"
                             
                             + "var LIST = Model;"
                             + "var list = Model; }";

                string bodyTemplate = null;

                if (!String.IsNullOrWhiteSpace(template))
                    bodyTemplate = Regex.Replace(template, _itemToHidePattern, String.Empty);

                if (!String.IsNullOrWhiteSpace(bodyTemplate))
                    bodyTemplate = Regex.Replace(bodyTemplate, _r_itemToHidePattern, String.Empty);

                string razorTemplate = headTemplate + Environment.NewLine + Environment.NewLine
                                     + bodyTemplate;

                ///////////////////
                // Parsing Start //
                ///////////////////

                result = Razor.Parse(razorTemplate, projectStaticItemDic);

                /////////////////
                // Parsing End //
                /////////////////
            }

            else if (projectRepeaterDic != null && projectRepeaterDic.Count > 0)
            {
                headTemplate = _firstHeader + "var repeater = Model;}";

                string bodyTemplate = null;

                if (!String.IsNullOrWhiteSpace(template))
                    bodyTemplate = Regex.Replace(template, _itemToHidePattern, String.Empty);

                if (!String.IsNullOrWhiteSpace(bodyTemplate))
                    bodyTemplate = Regex.Replace(bodyTemplate, _r_itemToHidePattern, String.Empty);

                string razorTemplate = headTemplate + Environment.NewLine + Environment.NewLine
                                     + bodyTemplate;

                ///////////////////
                // Parsing Start //
                ///////////////////

                result = Razor.Parse(razorTemplate, projectRepeaterDic);

                /////////////////
                // Parsing End //
                /////////////////
            }

            return result;
        }
    }
}
