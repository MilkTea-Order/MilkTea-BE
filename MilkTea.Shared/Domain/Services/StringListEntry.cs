namespace MilkTea.Shared.Domain.Services
{
    public class StringListEntry
    {
        private readonly Dictionary<string, List<string>> _Data = new();
        public bool HasData
        {
            get
            {
                foreach (var kvp in _Data)
                {
                    if (kvp.Value.Count > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        private readonly Dictionary<string, object> _Meta = new();

        //====META====
        public void AddMeta(string key, object? value)
        {
            if (value != null)
            {
                _Meta[key] = value;
            }
        }

        public object? GetMeta(string key)
        {
            return _Meta.TryGetValue(key, out var value) ? value : null;
        }

        // Get all meta with non-null values
        public Dictionary<string, object> GetMetaData()
        {
            Dictionary<string, object> vResult = [];
            foreach (var kvp in _Meta)
            {
                if (kvp.Value != null)
                {
                    vResult[kvp.Key] = kvp.Value;
                }
            }
            return vResult;
        }

        //====DATA====
        public void Add(string key, string? value)
        {
            if (value != null)
            {
                if (_Data.ContainsKey(key))
                {
                    _Data[key].Add(value);
                }
                else
                {
                    _Data[key] = [value];
                }
            }
        }

        public void Add(string key, List<string> value)
        {
            if (value != null && value.Count > 0)
            {
                if (_Data.ContainsKey(key))
                {
                    _Data[key].AddRange(value);
                }
                else
                {
                    _Data[key] = value;
                }
            }
        }


        public void RemoveKey(string key)
        {
            _Data.Remove(key);
        }

        public void RemoveValue(string value)
        {
            foreach (var kvp in _Data)
            {
                kvp.Value.Remove(value);
            }
        }

        // Get all data
        // If a key has only one value, return that value directly
        // If a key has multiple values, return the list of values
        public Dictionary<string, object> GetData()
        {
            Dictionary<string, object> result = new();

            foreach (var kvp in _Data)
            {
                if (kvp.Value == null || kvp.Value.Count == 0)
                    continue;

                if (kvp.Value.Count == 1)
                {
                    result[kvp.Key] = kvp.Value[0];
                }
                else
                {
                    result[kvp.Key] = kvp.Value;
                }
            }

            return result;
        }


        // Get all data values of _Data
        public List<string> GetAllDataValue()
        {
            List<string> vResult = [];
            foreach (var kvp in _Data)
            {
                if (kvp.Value.Count > 0)
                {
                    vResult.AddRange(kvp.Value);
                }
            }
            return vResult;
        }


        // Get all data with duplicate values
        public Dictionary<string, List<string>> FindDuplicateKey()
        {
            Dictionary<string, List<string>> vResult = [];
            foreach (var kvp in _Data)
            {
                if (kvp.Value.Count > 1)
                {
                    vResult[kvp.Key] = kvp.Value;
                }
            }
            return vResult;
        }

        // Clear all data in _Data
        public void Clear()
        {
            _Data.Clear();
        }

        // Convert data to JSON string
        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(GetData());
        }
    }
}
