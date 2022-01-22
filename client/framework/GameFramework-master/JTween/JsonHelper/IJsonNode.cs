namespace Json
{
    public interface IJsonNode
    {
        int Count { get; }

        object Value { get; }

        void Add(IJsonNode value);
        IJsonNode this[int index] { get; }

        void SetNode(string key, IJsonNode value);
        void SetString(string key, string value);
        void SetInt(string key, int value);
        void SetBool(string key, bool value);
        void SetDouble(string key, double value);
        void SetLong(string key, long value);
        void SetFloat(string key, float value);

        IJsonNode GetNode(string key);
        string GetString(string key);
        int GetInt(string key);
        bool GetBool(string key);
        double GetDouble(string key);
        long GetLong(string key);
        float GetFloat(string key);

        string ToJson();

        bool Contains(string key);

        void Clear();
    }
}
