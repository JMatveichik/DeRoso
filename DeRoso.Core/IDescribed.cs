namespace DeRoso.Core
{
    interface IDescribed
    {
        /// <summary>
        /// Название параметра
        /// </summary>        
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Описание параметра
        /// </summary>
        string Description
        {
            get;
            set;            
        }        
    }
}
