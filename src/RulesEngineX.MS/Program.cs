// See https://aka.ms/new-console-template for more information
using RulesEngineX.MS;

Console.WriteLine("Hello, World!");
DiscountWorkflow   discountWorkflow = new DiscountWorkflow();
await discountWorkflow.Invoke();

Console.Read();