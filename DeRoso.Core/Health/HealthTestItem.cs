namespace DeRoso.Core.Health
{
    public class HealthTestItem : ViewModelBase, IDescribed
    {

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название параметра
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                    return;

                _title = value;
                OnPropertyChanged();
            }
        }
        private string _title;


        /// <summary>
        /// Описание параметра
        /// </summary>
        public string Description
        {
            get => _desc;
            set
            {
                if (value == _desc)
                    return;

                _desc = value;
                OnPropertyChanged();
            }
        }
        private string _desc;
    }
}
