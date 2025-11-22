namespace ERMS.Enums
{
    public enum EventStatus
    {
        Scheduled,   // Agendado (criado, mas ainda não aberto)
        Open,        // Aberto para inscrições
        InProgress,  // Em andamento (evento acontecendo)
        Closed,      // Fechado (não aceita mais inscrições)
        Cancelled,   // Cancelado
        Completed    // Concluído/Finalizado
    }
} 