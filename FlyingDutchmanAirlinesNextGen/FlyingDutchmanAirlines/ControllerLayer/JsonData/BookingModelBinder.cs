using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FlyingDutchmanAirlines.ControllerLayer.JsonData;

public class BookingModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentException();
        }

        var result = await bindingContext.HttpContext.Request.BodyReader.ReadAsync();

        var buffer = result.Buffer;
        var body = Encoding.UTF8.GetString(buffer.FirstSpan);

        var data = JsonSerializer.Deserialize<BookingData>(body);
        
        bindingContext.Result = ModelBindingResult.Success(data);
    }
}
