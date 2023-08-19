using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryStringHandler
{
    public class QueryString
    {
        private readonly Dictionary<string, string> _data;
        /// <summary>
        /// Creates a new QueryString object
        /// </summary>
        /// <param name="url">a string the represents a URL</param>
        public QueryString(string url)
        {
            _data = ParseQueryString(url);
        }

        /// <summary>
        /// builds a QueryString from query string params
        /// </summary>
        /// <param name="parseMethod"> a delegate method that overrides the default method, if null $"{pair.Key}={pair.Value}" is used</param>
        /// <param name="prefixQuestionMark">decides if the QueryString will be prefixed by question mark or not</param>
        /// <returns></returns>
        public string ReturnQueryString(Func<KeyValuePair<string, string>, string> parseMethod = null, bool prefixQuestionMark = true)
        {
            var list = _data.Select(pair => parseMethod == null ? $"{pair.Key}={pair.Value}" : parseMethod(pair)).ToList();

            return (prefixQuestionMark ? "?" : string.Empty) + string.Join("&", list);
        }

        /// <summary>
        /// creates a query string from a dictionary
        /// </summary>
        /// <param name="data">data to create to a query string</param>
        /// <param name="prefixQuestionMark">decides should start with a question mark ot not </param>
        /// <returns></returns>
        public static string CreateQueryString(Dictionary<string, string> data, bool prefixQuestionMark = true)
        {
            var qs = string.Join("&", data.Where(d => !string.IsNullOrEmpty(d.Value)).Select(d => $"{d.Key}={d.Value}"));

            return (prefixQuestionMark ? "?" : "") + qs;
        }

        #region insert
      
        /// <summary>
        /// Inserts a new key to the QueryString, if the key is already exists the old value is replaced by the new value
        /// </summary>
        /// <param name="key">the key that we want insert</param>
        /// <param name="value">the value that we want to insert</param>
        /// <returns></returns>
        public QueryString Insert(string key, string value)
        {
            InsertKey(_data, key, value);
            return this;
        }
      
        /// <summary>
        /// Inserts several new keys to the QueryString, if a key is already exists the old value is replaced by the new value
        /// </summary>
        /// <param name="paramList">a list of key value pairs</param>
        /// <returns></returns>
        public QueryString Insert(Dictionary<string, string> paramList)
        {
            foreach (var item in paramList)
            {
                Insert(item.Key, item.Value);
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
            _data.Remove(key);
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
            if (_data.ContainsKey(oldKey))
            {
                var value = _data[oldKey];
                _data.Remove(oldKey);
                _data[newKey] = value;
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
        public QueryString UpdateValue(string key, string value )
        {
            UpdateKeyValue(_data, key, value);
            return this;
        }
        
        /// <summary>
        /// updates the values of severl key at once
        /// </summary>
        /// <param name="paramList">Key is the the param key, Value is the new value</param>
        /// <param name="duplicateValueMode">an enum, manages how duplicated key should be handeled (concat value, replase old value, keep old value)</param>
        /// <returns></returns>
        public QueryString UpdateValue(Dictionary<string, string> paramList )
        {
            foreach (var item in paramList)
            {
                UpdateValue(item.Key, item.Value );
            }

            return this;
        }

        #endregion

        public Dictionary<string, string> GetQueryStringParams()
        {
            return _data;
        }

        /// <summary>
        /// returns the value of a QueryString key
        /// </summary>
        /// <param name="url">the URL where we search the key in</param>
        /// <param name="key">the key whose value we want</param>
        /// <returns></returns>
        public static string GetQueryStringValue(string url, string key)
        {
            var data = ParseQueryString(url );
            var hasValue = data.TryGetValue(key, out var value);
            return hasValue ? value : null;
        }

        public static Dictionary<string, string> ParseQueryString(string url )
        {
            var data = new Dictionary<string, string>();

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
                InsertKey(data, key, value);
            }

            return data;
        }

        public string GetValue(string key)
        {
            var hasValue = _data.TryGetValue(key, out var value);
            return hasValue ? value : null;
        }

        #region Private

        private static void InsertKey(Dictionary<string, string> data, string key, string value)
        {
            if (data.ContainsKey(key))
            {
                UpdateKeyValue(data, key, value);
            }
            else
            {
                data.Add(key, value);
            }
        }

        private static void UpdateKeyValue(Dictionary<string, string> data, string key, string value)
        {
            data[key] = value;
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
            {
                url = newUrl;
            }

            return newUrl;
        }
        #endregion
    }
}
