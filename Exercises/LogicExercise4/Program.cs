// See https://aka.ms/new-console-template for more information

Logic rule = new Logic();

rule.AddRules(3,"foo");
rule.AddRules(4,"baz");
rule.AddRules(5,"bar");
rule.AddRules(4,"aaa");

rule.RemoveRules(8);
rule.RemoveRules(4);

rule.AddRules(7,"jazz");
rule.AddRules(4,"baz");
rule.AddRules(9,"huzz");

rule.ExecuteLogic(60);