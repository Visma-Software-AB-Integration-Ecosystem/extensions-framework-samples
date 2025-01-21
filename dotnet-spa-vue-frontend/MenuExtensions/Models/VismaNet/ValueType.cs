namespace MenuExtensions.Models.VismaNet;

/// <summary>
/// A generic value type that can be used to wrap nullable value types.
/// This is used a lot in the Visma Net API.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Value"></param>
public record ValueType<T>(T? Value);
