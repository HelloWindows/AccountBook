using System;

namespace Json
{
    public static class JsonHelper
    {
        private static IJsonHelper helper;

        public static void SetJsonHelper(IJsonHelper helper)
        {
            JsonHelper.helper = helper;
        }

        public static IJsonNode CreateNode() 
        {
            if (helper == null)
            {
                throw new Exception("JsonHelper helper is null!");
            }
            return helper.CreateNode();
        }

        public static IJsonNode CreateNode(string str)
        {
            if (helper == null)
            {
                throw new Exception("JsonHelper helper is null!");
            }
            return helper.CreateNode(str);
        }
    }
}
