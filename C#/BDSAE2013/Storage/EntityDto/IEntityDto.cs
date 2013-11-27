namespace Storage.EntityDto
{
    public interface IEntityDto
    {
        int Id { get; set; }
        EntityState State { get; set; }
    }
}
