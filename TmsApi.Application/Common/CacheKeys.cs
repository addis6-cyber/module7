namespace TmsApi.Application.Common;

public static class CacheKeys
{
    //public static string StudentById(int id) => $"students:{id}";
    public static string Student(int id) => $"Student-{id}";
}