using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryStringHandler
{

    public class QueryString
    {
        Dictionary<string, string> data;
        /// <summary>
        /// Creates a new QueryString object
        /// </summary>
        /// <param name="url">a string the represents a URL</param>
        /// <param name="duplicateValueMode">an enum, manages how duplicated key should be handeled (concat values, replase old values, keep old values)</param>
        public QueryString(string url, DuplicateKeyMode duplicateValueMode = DuplicateKeyMode.Concat)
        {
            data = ParseQueryString(url, duplicateValueMode);
        }

        /// <summary>
        /// builds a QueryString from query string params
        /// </summary>
        /// <param name="parseMethod"> a delegate method that overrides the deafult method, if null $"{pair.Key}={pair.Value}" is used</param>
        /// <param name="preffixQuestionMark">decides if the QueryString will be preffixed by question mark or not</param>
        /// <returns></returns>
        public string RetunrQueryString(Func<KeyValuePair<string, string>, string> parseMethod = null, bool preffixQuestionMark = true)
        {
            var list = data.Select(pair => parseMethod == null ? $"{pair.Key}={pair.Value}" : parseMethod(pair)).ToList();

            return (preffixQuestionMark ? "?" : string.Empty) + string.Join("&", list);
        }

        #region insert
        /// <summary>
        /// Inserts a new key to the QueryString, if the key is already exists the value is concated to the old value 
        /// </summary>
        /// <param name="key">the key that we want insert</param>
        /// <param name="value">the value that we want to insert</param>
        /// <returns></returns>
        public QueryString InsertOrConcat(string key, string value)
        {
            InsertKey(data, key, value, DuplicateKeyMode.Concat);
            return this;
        }
        /// <summary>
        /// Inserts a new key to the QueryString, if the key is already exists the old value is relased by the new value
        /// </summary>
        /// <param name="key">the key that we want insert</param>
        /// <param name="value">the value that we want to insert</param>
        /// <returns></returns>
        public QueryString InsertOrReplase(string key, string value)
        {
            InsertKey(data, key, value, DuplicateKeyMode.Replase);
            return this;
        }
        /// <summary>
        /// Inserts a new key to the QueryString, if the key is already exists the old value is kept
        /// </summary>
        /// <param name="key">the key that we want insert</param>
        /// <param name="value">the value that we want to insert</param>
        /// <returns></returns>
        public QueryString InsertOrKeepOld(string key, string value)
        {
            InsertKey(data, key, value, DuplicateKeyMode.KeepOld);
            return this;
        }

        /// <summary>
        /// Inserts several new keys to the QueryString, if a key is already exists the value is concated to the old value
        /// </summary>
        /// <param name="paramList">a list of key value pairs</param>
        /// <returns></returns>
        public QueryString InsertOrConcat(Dictionary<string, string> paramList)
        {
            foreach (var item in paramList)
            {
                InsertOrConcat(item.Key, item.Value);
            }
            return this;

        }
        /// <summary>
        /// Inserts several new keys to the QueryString, if a key is already exists the old value is relased by the new value
        /// </summary>
        /// <param name="paramList">a list of key value pairs</param>
        /// <returns></returns>
        public QueryString InsertOrReplase(Dictionary<string, string> paramList)
        {
            foreach (var item in paramList)
            {
                InsertOrReplase(item.Key, item.Value);
            }
            return this;
        }
        /// <summary>
        /// Inserts several new keys to the QueryString, if a key is already exists the old value is kept
        /// </summary>
        /// <param name="paramList">a list of key value pairs</param>
        /// <returns></returns>
        public QueryString InsertOrKeepOld(Dictionary<string, string> paramList)
        {
            foreach (var item in paramList)
            {
                InsertOrKeepOld(item.Key, item.Value);
            }
            return this;
        }
        #endregion

        #region delete
        /// <summary>
        /// removes a key from the QueryString
        /// </summary>
        /// <param name="key">the key we want to remove</param>
        /// <returns></returns>
        public QueryString DeleteKey(string key)
        {
            data.Remove(key);
            return this;
        }

        /// <summary>
        /// removes a list of key from the QueryString
        /// </summary>
        /// <param name="keys">a list of keys to remove</param>
        /// <returns></returns>
        public QueryString DeleteKeys(List<string> keys)
        {
            foreach (var key in keys)
            {
                DeleteKey(key);
            }

            return this;
        }
        #endregion

        #region update

        /// <summary>
        /// renames the a key in the QueryString, the value is unchanged
        /// </summary>
        /// <param name="oldKey">the key we want to rename</param>
        /// <param name="newKey">the new name of the key</param>
        /// <returns></returns>
        public QueryString RenameKey(string oldKey, string newKey)
        {
            if (data.ContainsKey(oldKey))
            {
                var value = data[oldKey];
                data.Remove(oldKey);
                data[newKey] = value;
            }

            return this;
        }

        /// <summary>
        /// renames several keys in the QueryString, the values are unchanged
        /// </summary>
        /// <param name="paramList">Key is the old key, Value is the new Key</param>
        /// <returns></returns>
        public QueryString RenameKeys(Dictionary<string, string> paramList)
        {
            foreach (var item in paramList)
            {
                RenameKey(item.Key, item.Value);
            }

            return this;
        }

        /// <summary>
        /// updates the value of a QueryString key
        /// </summary>
        /// <param name="key">the key whos value we want to change</param>
        /// <param name="value">the new value</param>
        /// <param name="duplicateValueMode">an enum, manages how duplicated key should be handeled (concat value, replase old value, keep old value)</param>
        /// <returns></returns>
        public QueryString UpdateValue(string key, string value, DuplicateKeyMode duplicateValueMode = DuplicateKeyMode.Concat)
        {
            UpdateKeyValue(data, key, value, duplicateValueMode);
            return this;
        }
        /// <summary>
        /// concats a new value to an exsiting value
        /// </summary>
        /// <param name="key">the key whos value we want to update</param>
        /// <param name="value">the new value</param>
        /// <returns></returns>
        public QueryString ConcatValue(string key, string value)
        {
            UpdateKeyValue(data, key, value, DuplicateKeyMode.Concat);
            return this;
        }

        /// <summary>
        /// replases a QueryString value
        /// </summary>
        /// <param name="key">the key whos value we want to update</param>
        /// <param name="value">the new value</param>
        /// <returns></returns>
        public QueryString ReplaseValue(string key, string value)
        {
            UpdateKeyValue(data, key, value, DuplicateKeyMode.Replase);
            return this;
        }

        /// <summary>
        /// updates the values of severl key at once
        /// </summary>
        /// <param name="paramList">Key is the the param key, Value is the new value</param>
        /// <param name="duplicateValueMode">an enum, manages how duplicated key should be handeled (concat value, replase old value, keep old value)</param>
        /// <returns></returns>
        public QueryString UpdateValue(Dictionary<string, string> paramList, DuplicateKeyMode duplicateValueMode = DuplicateKeyMode.Concat)
        {
            foreach (var item in paramList)
            {
                UpdateValue(item.Key, item.Value, duplicateValueMode);
            }

            return this;
        }

        #endregion

        public Dictionary<string, string> GetQueryStringParams()
        {
            return data;
        }

        /// <summary>
        /// returns the value of a QueryString key
        /// </summary>
        /// <param name="url">the URL where we search the key in</param>
        /// <param name="key">the key whose value we want</param>
        /// <returns></returns>
        public static string GetQueryStringValue(string url, string key)
        {
            var data = ParseQueryString(url, DuplicateKeyMode.Concat);
            return data.ContainsKey(key) ? data[key] : null;
        }

        public static Dictionary<string, string> ParseQueryString(string url, DuplicateKeyMode duplicateValueMode = DuplicateKeyMode.Concat)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            url = DecodeUrlString(url);

            if (!url.Contains("?"))
            {
                url += "?" + url;
            }

            var urlParts = url.Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (string.IsNullOrEmpty(urlParts[1]))
            {
                return data;
            }

            var queryStringParams = urlParts[1].Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


            foreach (var qsParam in queryStringParams)
            {
                var paramParts = qsParam.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var key = paramParts[0];
                var value = paramParts.Length == 2 ? paramParts[1] : string.Empty;
                InsertKey(data, key, value, duplicateValueMode);
            }

            return data;
        }

        #region Private
        private string GetQueryStringValue(string key)
        {
            return data.ContainsKey(key) ? data[key] : null;
        }
        private static void InsertKey(Dictionary<string, string> data, string key, string value, DuplicateKeyMode duplicateValueMode)
        {
            if (data.ContainsKey(key))
            {
                UpdateKeyValue(data, key, value, duplicateValueMode);
            }
            else
            {
                data.Add(key, value);
            }
        }

        private static void UpdateKeyValue(Dictionary<string, string> data, string key, string value, DuplicateKeyMode duplicateValueMode)
        {
            switch (duplicateValueMode)
            {
                case DuplicateKeyMode.Concat:
                    data[key] += "," + value;
                    break;
                case DuplicateKeyMode.Replase:
                    data[key] = value;
                    break;
                case DuplicateKeyMode.KeepOld:
                    //do nothing, old value remains
                    break;
                default:
                    break;
            }
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }
        #endregion
    }
}
