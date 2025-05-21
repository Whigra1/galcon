public static class RoomInfo
{
    public static int Id { get; set; }
    public static string Name { get; set; }
    public static string InvitatationToken { get; set; }
    
    public static bool IsHost { get; set; }

    public static void Clear()
    {
        Id = 0;
        Name = "";
        InvitatationToken = "";
        IsHost = false;
    } 
}
