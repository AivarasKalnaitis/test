using Amazon.SQS;
using TrafiTrialDay.AwsSqs;
using TrafiTrialDay.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ICarsRepository, CarsRepository>(); // gali but singleton
builder.Services.AddSingleton<IBookingsRepository, BookingsRepository>();

builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddTransient<IAWSSQSService, AWSSQSService>();
builder.Services.AddTransient<IAWSSQSHelper, AWSSQSHelper>(); // transient kada tikrai reik pagal tai kas naudojama ir kur, jei butu nauodjama locks butu problemu jei bus static lockas nebent

builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseRouting();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

// clean architecture (yra repo githube, overkill kai mazas), daugiasluoskne, daugiau skaidyt i atskirus projectus, kaip geriau testuose koda handlint, kur gaima mock daugiau isidet testuojant, 
// keli projektai data, servisai su bisnio, persistance trecioj vietoj
// nereik su struktura persistengt ant tokiu mazu tasks
// daugiau concurency pasiziuret, pagalvot
// zinot tradeoffs savo