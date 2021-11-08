using DeRoso.Core.Health;

namespace DeRoso.Core.Data
{
    public interface IResultsSaver
    {

        /// <summary>
        /// Сохранение результатов тесирования
        /// </summary>
        /// <param name="results"></param>
        bool Save(HealthTestReport report, bool showResults);

    }
}
