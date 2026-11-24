namespace EvaluaIndividuos.Database
{
    public interface IDictaminador
    {
        long InsertRow(IIdentity row);
        void InsertRows(Object[] rows);
    }
}
