namespace DistrictStatisticsManagement.Enums
{
    /// <summary>
    /// Форма собственности предприятия
    /// </summary>
    public enum OwnershipType
    {
        /// <summary>
        /// Государственно-федеральная
        /// </summary>
        StateFederal,
        
        /// <summary>
        /// Муниципально-городская
        /// </summary>
        MunicipalCity,
        
        /// <summary>
        /// ТОО (Товарищество с ограниченной ответственностью)
        /// </summary>
        LimitedLiabilityPartnership,
        
        /// <summary>
        /// Частная
        /// </summary>
        Private,
        
        /// <summary>
        /// Акционерная
        /// </summary>
        JointStock
    }
}