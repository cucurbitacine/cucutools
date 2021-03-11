namespace CucuTools.Raycasts.Effects
{
    /// <summary>
    /// Эффект для рейкаста
    /// </summary>
    public interface IRaycastEffect
    {
        /// <summary>
        /// Работает ли эффект
        /// </summary>
        bool IsEnabled { get; set; } 
        
        /// <summary>
        /// Рейкастер
        /// </summary>
        IRaycastEntity Raycaster { get; }
        
        /// <summary>
        /// Обновить эффект
        /// </summary>
        void UpdateEffect();
    }
}