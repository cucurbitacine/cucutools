using UnityEngine;

namespace CucuTools
{
    /// <summary>
    /// Рейкастер
    /// </summary>
    public interface IRaycastEntity
    {
        /// <summary>
        /// Работает или нет рейкакстер
        /// </summary>
        bool IsEnabled { get; set; }
        
        /// <summary>
        /// Расстояние рейкастинга
        /// </summary>
        float Distance { get; set; }
        
        /// <summary>
        /// Маска видимых объектов
        /// </summary>
        LayerMask LayerMask { get; set; }

        /// <summary>
        /// Рейкаст
        /// </summary>
        /// <param name="hit">Информация о "ударе" рейкаста</param>
        /// <returns>Успешность</returns>
        bool Raycast(out RaycastHit hit);
    }
}