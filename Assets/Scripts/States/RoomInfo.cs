public static class RoomInfo
{
    public static string Id { get; set; }
    public static string Name { get; set; }
    public static string InvitatationToken { get; set; }
    
    public static bool IsHost { get; set; }

    public static void Clear()
    {
        Id = "";
        Name = "";
        InvitatationToken = "";
        IsHost = false;
    } 
}
