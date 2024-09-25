using RulesEngine.Extensions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RulesEngineX.MS
{
    public class DiscountWorkflow
    {
        public async Task Invoke()
        {
            try
            {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "Discount.json", SearchOption.AllDirectories);
                if (files == null || files.Length == 0)
                    throw new Exception("Rules not found.");

                var fileData = File.ReadAllText(files[0]);
                var workflows = JsonSerializer.Deserialize<List<Workflow>>(fileData);
                //workflows[0].Rules.ToList()[0].RuleExpressionType = RuleExpressionType.LambdaExpression;


                dynamic input1 = new
                {
                    name = "hello",
                    email = "abcd@xyz.com",
                    creditHistory = "good",
                    country = "india",
                    loyaltyFactor = 3,
                    totalPurchasesToDate = 10000
                };
                dynamic input2 = new { totalOrders = 5, recurringItems = 2 };
                dynamic input3 = new { noOfVisitsPerMonth = 10, percentageOfBuyingToVisit = 15 };

                var inputs = new object[]
                    {
                    input1,
                    input2,
                    input3
                    };
                if (workflows is null)
                    return;

                var bre = new RulesEngine.RulesEngine(workflows.ToArray());


                string discountOffered = "No discount offered.";
                List<RuleResultTree> resultList = await bre.ExecuteAllRulesAsync("Discount", inputs);
                var successRules = resultList.Where(_ => _.IsSuccess).ToList();

                resultList.OnSuccess((eventName) =>
                {
                    discountOffered = $"Discount offered is {eventName} % over MRP.";
                });

                resultList.OnFail(() =>
                {
                    discountOffered = "The user is not eligible for any discount.";
                });

                Console.WriteLine(discountOffered);
            }
            catch (Exception ex)
            {

            }


            await Task.FromResult(true);
        }
    }

}
