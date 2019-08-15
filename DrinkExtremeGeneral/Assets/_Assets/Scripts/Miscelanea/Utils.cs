using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DrinkExtreme
{

    public static class Utils
    {
        //App configuration
        public static readonly bool isProApp = false;
        public static readonly string defaultLanguage = "es-es";
        public static readonly List<string> defaultPlayers = new List<string>();
        public static readonly int minDifficulty = 1;
        public static readonly int maxDifficulty = 3;
        public static readonly int defaultDifficulty = maxDifficulty;
        public static readonly bool defaultRandomSentencesAllowed = true;
        public static readonly bool defaultCustomSentencesAllowed = true;
        public static readonly int minNumberOfTurnsToRandomSentence = 7;
        public static readonly int maxNumberOfTurnsToRandomSentence = 15;
        public static readonly int NumberOfRandomSentenceToCustomSentence = 3;
        public static readonly int secondsBetweenTimeCountings = 5; //Every how much time the time in mode counter increases
        public static readonly int timeInModeToShowRatePanel = 4*60;
        public static readonly int timeInModeToShowInterstitialAd = 1*60;


        //Localization
        public static readonly string localizationFilesPath = "LocalizationFiles/";
        public static readonly string translationsFilename = "LocalizationFile - Translations";
        public static readonly string languagePriorityFilename = "LocalizationFile - Languages Priority";
        public static readonly string languagesInfoFilename = "LocalizationFile - LangInfo";
        public static readonly int columnNumberOfLanguageImgInLangInfoFile = 1; //Column "B" in the document
        public static readonly int columnNumberOfLanguageNameInLangInfoFile = 2; //Column "C" in the document
        public static readonly string mainLanguage = "es-es";
        public static readonly string notLocalizableSentenceIdentifier = "#P##P##P##P##P##P#";
        public static readonly string localizedImagesPath = "LocalizableSprites/";
        public static readonly string localizationFileNameColumnId = "Id";
        public static readonly string localizationFileNameColumnType = "Type";
        public static readonly string localizationFileNameColumnDifficulty = "NivelPicante";
        public static readonly string endOfFileTag = "<END>";
        public static readonly string indicatorOfSentenceAddedByUser = "%";
        public static readonly string indicatorOfPlayerNameInSentence = "#P#";

        public static readonly string beginingOfRandomSentenceId = "RS";
        public static readonly string beginingOfCustomSentenceId = "CS";
        public static readonly string tooMuchPlayersTextId = "TooMuchPlayers";
        public static readonly string notEnoughPlayersTextId = "NotEnoughPlayers";
        public static readonly string notEnoughTypesOfSentences = "NotEnoughTypesOfSentences";


        //Links
        public static readonly string android = "https://play.google.com/store/apps/details?id=com.burtons.ProductName";
        public static readonly string iOs = "https://play.google.com/store/apps/details?id=com.burtons.drinkExtremePro";
        public static readonly string instagram = "instagram://user?username=duckteamstudio";
        public static readonly string twitter = "twitter://user?screen_name=DrinkExtreme";
        public static readonly string web = "https://drinkextreme.com/";
        public static readonly string store = ""; //Currently non existent
        public static readonly string rateAndroid = android;
        public static readonly string rateiOs = iOs;
        public enum Link { None, android, iOs, instagram, twitter, web, store, rateAndroid, rateiOs }
        public static void OpenExternalPredefinedLink(Link link)
        {
            switch (link)
            {
                case Link.android:
                    OpenExternalLink(android);
                    break;
                case Link.iOs:
                    OpenExternalLink(iOs);
                    break;
                case Link.instagram:
                    OpenExternalLink(instagram);
                    break;
                case Link.twitter:
                    OpenExternalLink(twitter);
                    break;
                case Link.web:
                    OpenExternalLink(web);
                    break;
                case Link.store:
                    OpenExternalLink(store);
                    break;
                case Link.rateAndroid:
                    OpenExternalLink(rateAndroid);
                    break;
                case Link.rateiOs:
                    OpenExternalLink(rateiOs);
                    break;
                default:
                    Debug.LogError("Unknown lnk: " + link);
                    break;
            }
        }
        public static Link GetLink(string type)
        {
            switch (type.ToUpper())
            {
                case "MARKET":
#if UNITY_ANDROID
                    return (Utils.Link.android);
#elif UNITY_IOS 
                    return (Utils.Link.iOs);
#endif
                case "ANDROID":
                    return (Utils.Link.android);
                case "IOS":
                    return (Utils.Link.iOs);
                case "INSTAGRAM":
                    return (Utils.Link.instagram);
                case "TWITTER":
                    return (Utils.Link.twitter);
                case "WEB":
                    return (Utils.Link.web);
                case "STORE":
                    return (Utils.Link.store);
                case "RATE":
#if UNITY_ANDROID
                    return (Utils.Link.rateAndroid);
#elif UNITY_IOS
                    return (Utils.Link.rateiOs);
#endif
                case "RATEANDROID":
                    return (Utils.Link.rateAndroid);
                case "RATEIOS":
                    return (Utils.Link.rateiOs);
                default:
                    Debug.LogError("'" + type.ToUpper() + "' is a not recognized type of link");
                    break;
            }
            return Link.None;
        }

        //Resources
        public static readonly string prefabsPath = "Prefabs/";
        public static readonly string nameOfGeneralGUIPrefab = "GeneralGUI";

        //Saves
        public static readonly string savesPath = Application.persistentDataPath;
        public static readonly string dataSeparator = "/./*ç*#p#*ç*/./";
        public static readonly string savesExtension = ".de";
        public static readonly string languageSavename = "language";
        public static readonly string playersSaveName = "players";
        public static readonly string difficultySavename = "difficulty";
        public static readonly string allowedTypesOfSentencesSavename = "allowedTypesOfSentences";
        public static readonly string randomSentencesAllowedSavename = "randomSentencesAllowed";
        public static readonly string rateAppShowSavename = "appvalorada";
        public static readonly string showAdsSavename = "showads";
        public static readonly string customSentencesAllowedSavename = "customSentencesAllowed";
        public static readonly string lastIdOfCustomSentenceSavename = "lastIdOfCustomSentence";
        public static readonly string customSentencesFilename = "CustomSentencesFile";

        //TypesOfSentences
        public enum TypeOfSentence { Adolescente, Sexualidad, Vergüenza, Instituto, Alcohol, Trabajo, Asquerosidad };
        public static string GetTypeOfSentenceStringOf(TypeOfSentence typeOfSentence)
        {
            switch (typeOfSentence)
            {
                case TypeOfSentence.Adolescente: return Enum.GetName(typeof(TypeOfSentence), TypeOfSentence.Adolescente);
                case TypeOfSentence.Sexualidad: return Enum.GetName(typeof(TypeOfSentence), TypeOfSentence.Sexualidad);
                case TypeOfSentence.Vergüenza: return Enum.GetName(typeof(TypeOfSentence), TypeOfSentence.Vergüenza);
                case TypeOfSentence.Instituto: return Enum.GetName(typeof(TypeOfSentence), TypeOfSentence.Instituto);
                case TypeOfSentence.Alcohol: return Enum.GetName(typeof(TypeOfSentence), TypeOfSentence.Alcohol);
                case TypeOfSentence.Trabajo: return Enum.GetName(typeof(TypeOfSentence), TypeOfSentence.Trabajo);
                case TypeOfSentence.Asquerosidad: return Enum.GetName(typeof(TypeOfSentence), TypeOfSentence.Asquerosidad);
                default: Debug.LogError("Type of sentence " + typeOfSentence.ToString() + " not found"); return null;
            }
        }

        //Warnings and info
        public static readonly string repeatedPlayerText = "RepeatedPlayerText";
        public static readonly string repeatedCustomSentenceText = "RepeatedCustomSentenceText";

        //Conditions
        public enum Condition { None, TerminosDeUso, sendDataToDB }
        public static string GetConditionStringOf(Condition condition)
        {
            switch (condition)
            {
                case Condition.TerminosDeUso: return "TERMINOSDEUSO";
                case Condition.sendDataToDB: return "SENDDATATODB";
                default: Debug.LogError("Condition " + condition.ToString() + " not found"); return null;
            }
        }

        //Miscelanea
        public static readonly int nullVal = -1;
        public static readonly int falseVal = 0;
        public static readonly int trueVal = 1;

        //Useful methods
        public static string[] CsvLineToArray(string line)
        {
            string[] returnArr = (Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"));
            for (int col = 0; col < returnArr.Length; col++)
            {
                returnArr[col] = returnArr[col].Replace("\"\"", "*ç!¡ö*"); //To be able to have double quotes in texts
                returnArr[col] = returnArr[col].Replace("\"", "");
                returnArr[col] = returnArr[col].Replace("*ç!¡ö*", "\""); //To be able to have double quotes in texts 
            }
            return returnArr;
        }
        public static int GetColumNumberWithText(string searchingText, string columnsSeparatedWithComas)
        {
            if (string.IsNullOrEmpty(searchingText))
            {
                Debug.LogWarning("The searched text is null or empty");
                return -1;
            }

            string[] columns = CsvLineToArray(columnsSeparatedWithComas);
            for (int column = 0; column < columns.Length; column++)
            {
                if (String.Equals(columns[column], searchingText, StringComparison.InvariantCultureIgnoreCase))
                    return column;
            }

            Debug.LogWarning("'" + searchingText + "' not found in '" + columnsSeparatedWithComas + "'");
            return -1;
        }

        private static void OpenExternalLink(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("URL Not implemented. Trying to open an empty URL");
            }
            else
            {
                Debug.Log("Opening " + url);
                Application.OpenURL(url);
            }
        }

        public static List<string> Trim(List<string> stringList)
        {
            List<string> trimmed = new List<string>();
            foreach (string str in stringList)
                trimmed.Add(str.Trim());
            return trimmed;
        }
        public static string[] Trim(string[] stringArray)
        {
            string[] trimmed = new string[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                trimmed[i] = stringArray[i].Trim();
            return trimmed;
        }

        public static string GetSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                //case SystemLanguage.Afrikaans:
                //case SystemLanguage.Arabic:
                //case SystemLanguage.Basque:
                //case SystemLanguage.Belarusian:
                //case SystemLanguage.Bulgarian:
                //case SystemLanguage.Catalan:
                //case SystemLanguage.Chinese:
                //case SystemLanguage.Czech:
                //case SystemLanguage.Danish:
                //case SystemLanguage.Dutch:
                case SystemLanguage.English: return LocalizationManager.instance.GetMainDialectOfLanguage("en-us");
                //case SystemLanguage.Estonian:
                //case SystemLanguage.Faroese:
                //case SystemLanguage.Finnish:
                //case SystemLanguage.French:
                //case SystemLanguage.German:
                //case SystemLanguage.Greek:
                //case SystemLanguage.Hebrew:
                //case SystemLanguage.Hungarian:
                //case SystemLanguage.Icelandic:
                //case SystemLanguage.Indonesian:
                //case SystemLanguage.Italian:
                //case SystemLanguage.Japanese:
                //case SystemLanguage.Korean:
                //case SystemLanguage.Latvian:
                //case SystemLanguage.Lithuanian:
                //case SystemLanguage.Norwegian:
                //case SystemLanguage.Polish:
                //case SystemLanguage.Portuguese:
                //case SystemLanguage.Romanian:
                //case SystemLanguage.Russian:
                //case SystemLanguage.SerboCroatian:
                //case SystemLanguage.Slovak:
                //case SystemLanguage.Slovenian:
                case SystemLanguage.Spanish: return LocalizationManager.instance.GetMainDialectOfLanguage("es-es");
                //case SystemLanguage.Swedish:
                //case SystemLanguage.Thai:
                //case SystemLanguage.Turkish:
                //case SystemLanguage.Ukrainian:
                //case SystemLanguage.Vietnamese:
                //case SystemLanguage.ChineseSimplified:
                //case SystemLanguage.ChineseTraditional:
                //case SystemLanguage.Unknown:
                default: return Utils.defaultLanguage;
            }
        }

        public static T ParseEnum<T>(string stringValue)
        {
            //Call using: Utils.ParseEnum<Utils.TypeOfSentence>(typeString);
            //return the value of the desired enum with the typeString name
            return (T)Enum.Parse(typeof(T), stringValue, true);
        }

        public static string[] getStringSeparated(string originalString)
        {
            return originalString.Split(new[] { Utils.dataSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string getStringsUnited(string[] strings)
        {
            string returnString = "";
            foreach (string str in strings)
            {
                returnString += str + Utils.dataSeparator;
            }
            return returnString;
        }

        public static string getStringsUnited(List<string> strings)
        {
            return getStringsUnited(strings.ToArray());
        }

        public static List<LocalizedText> RandomizeOrder(List<LocalizedText> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                LocalizedText temp = list[i];
                int randomIndex = UnityEngine.Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
            return list;
        }

    }

}

