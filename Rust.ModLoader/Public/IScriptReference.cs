public interface IScriptReference
{
    void InvokeProcedure(string methodName);
    T0 InvokeFunction<T0>(string methodName);

    void InvokeProcedure<T0>(string methodName, T0 arg0);
    T1 InvokeFunction<T0, T1>(string methodName, T0 arg0);

    void InvokeProcedure<T0, T1>(string methodName, T0 arg0, T1 arg1);
}
