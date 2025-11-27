using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddValidatorsFromAssemblyContaining<ProdutoCreateDtoValidator>();

var app = builder.Build();

//Get Listar todos os produtos.
app.MapGet("/produtos", async (IProdutoService service, CancellationToken ct) =>
{
    return Results.Ok(await service.ListarAsync(ct));
});
// Get que busca por id.
app.MapGet("/produtos/{id}", async (int id, IProdutoService service, CancellationToken ct) =>
{
    var produto = await service.ObterAsync(id, ct);
    return produto != null ? Results.Ok(produto) : Results.NotFound();
});
//post criar produto
app.MapPost("/produtos", async (
    ProdutoCreateDto produtoDto
    , IProdutoService service
    , CancellationToken ct
    , IValidator<ProdutoCreateDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(produtoDto, ct);
    if(!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    var produto = await service.CriarAsync(produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, produtoDto.Estoque, ct);
    return Results.Created($"/produtos/{produto.Id}", produto);
});
//Put atualização completa do produto
app.MapPut("/produtos/{id}", async (int id, Produto produto, IProdutoService service, CancellationToken ct) =>
{
    var produtoAtualizado = await service.AtualizarAsync(id, produto);
    if (produtoAtualizado == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(produtoAtualizado);
});
//Patch atualiza parcialmente o produto
app.MapPatch("/produtos/{id}", async (int id, Produto produto, IProdutoService service, CancellationToken ct) =>
{
    var produtoAtualizado = await service.AtualizarParcialAsync(id, produto);
    if (produtoAtualizado == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(produtoAtualizado);
});
// Delete produto
app.MapDelete("/produtos/{id}", async (int id, IProdutoService service, CancellationToken ct) =>
{
    var produto = await service.ObterAsync(id, ct);
    if (produto == null) return Results.NotFound();
    await service.RemoverAsync(id, ct);
    return Results.NoContent();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();


app.Run();

