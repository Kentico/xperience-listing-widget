namespace DancingGoat.Helpers.Generator
{
    /// <summary>
    /// Structure is used to box utm info about activity that should be generated. 
    /// Generator will generate number of Activities defined by Count and with specified UtmSource and UtmContent
    /// </summary>
    internal struct ActivityDataParameters
    {
        public string UtmSource
        {
            get;
            set;
        }


        public string UtmContent
        {
            get;
            set;
        }


        public int Count
        {
            get;
            set;
        }
    }
}