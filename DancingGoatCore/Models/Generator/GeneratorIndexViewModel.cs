namespace DancingGoat.Models
{
    public class GeneratorIndexViewModel
    {
        public bool DisplaySuccessMessage
        {
            get;
            set;
        }


        public bool IsAuthorized
        {
            get;
            set;
        } = true;


        public string ABTestErrorMessage
        {
            get;
            internal set;
        }
    }
}