using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Feautures.VentaC.Commands
{
    public class GenerarNotaPDF : IRequest<Stream>
    {
        public long Ventaid { get; set; }
    }

    public class GenerarFacturaCommandHandler : IRequestHandler<GenerarNotaPDF, Stream>
    {
        private readonly IReadOnlyRepositoryAsync<Venta> _ventaRepository;

        public GenerarFacturaCommandHandler(IReadOnlyRepositoryAsync<Venta> ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        public async Task<Stream> Handle(GenerarNotaPDF request, CancellationToken cancellationToken)
        {
            var venta = await _ventaRepository.FirstOrDefaultAsync(new VentaSpecification(request.Ventaid));
            if (venta == null)
                throw new ApiException($"Venta con Id {request.Ventaid} no encontrada");

            var stream = GenerarFacturaPdf(venta);
            return stream;
        }

        public Stream GenerarFacturaPdf(Venta venta)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(60);

                    // Encabezado
                    page.Header().ShowOnce().Column(headerCol =>
                    {
                        headerCol.Item().Row(row =>
                        {
                            byte[] logoBytes = File.ReadAllBytes("C:\\Users\\PC\\Pictures\\6TO\\PI\\BACKEND\\centro_emprendimiento_restful\\Application\\Imagenes\\Escudo_Uniandes.png");
                            byte[] logoCiemi = File.ReadAllBytes("C:\\Users\\PC\\Pictures\\6TO\\PI\\BACKEND\\centro_emprendimiento_restful\\Application\\Imagenes\\Ciemi.png");

                            row.ConstantItem(130).Height(60).Image(logoCiemi);

                            row.RelativeItem().PaddingTop(15).Column(col =>
                            {
                                col.Item().AlignCenter().PaddingBottom(5).Text(venta.Negocio.nombre.ToUpper()).Bold().FontSize(16);
                                col.Item().AlignCenter().PaddingBottom(5).Text(venta.Negocio.direccion).FontSize(9);
                                col.Item().AlignCenter().PaddingBottom(5).Text(venta.Negocio.telefono).FontSize(9);
                                col.Item().AlignCenter().PaddingBottom(5).Text("codigo@example.com").FontSize(9);
                            });

                            row.ConstantItem(130).AlignRight().Height(70).Image(logoBytes);
                        });

                        headerCol.Item().LineHorizontal(0.5f).LineColor("#000000");

                        headerCol.Item().PaddingTop(10)
                            .AlignCenter()
                            .Text($"Nota de venta N.º {venta.Id.ToString()}")
                            .Bold().FontSize(12).FontColor(Colors.Black);
                    });

                    // Contenido
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        // Datos del cliente
                        col1.Item().Column(col2 =>
                        {
                            col2.Item().Text("Datos del cliente").Bold();
                            col2.Spacing(3, QuestPDF.Infrastructure.Unit.Millimetre);

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Nombres: ").SemiBold().FontSize(10);
                                txt.Span(venta.Cliente.Nombres).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("DNI: ").SemiBold().FontSize(10);
                                txt.Span(venta.Cliente.Identificacion).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Dirección: ").SemiBold().FontSize(10);
                                txt.Span(venta.Cliente.Direccion).FontSize(10);
                            });
                        });

                        col1.Item().PaddingTop(15);

                        // Tabla de productos
                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#0059b3").Padding(2).Text("Producto").FontColor("#ffffff");
                                header.Cell().Background("#0059b3").Padding(2).Text("Precio Unit").FontColor("#ffffff");
                                header.Cell().Background("#0059b3").Padding(2).Text("Cantidad").FontColor("#ffffff");
                                header.Cell().Background("#0059b3").Padding(2).Text("Total").FontColor("#ffffff");
                            });

                            foreach (var item in venta.Detalles)
                            {
                                var total = item.Cantidad * item.Precio;

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.Producto.Nombre).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text($"$ {item.Precio:F2}").FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.Cantidad.ToString()).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).AlignRight().Text($"$ {total:F2}").FontSize(10);
                            }
                        });

                        // Totales
                        col1.Item().AlignRight().Width(200).PaddingTop(20)
                            .Border(1).BorderColor(Colors.Grey.Darken2)
                            .Background(Colors.Grey.Lighten3).Padding(10)
                            .Column(totals =>
                            {
                                totals.Spacing(5);

                                totals.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text("Subtotal:").Bold().FontSize(12);
                                    row.ConstantItem(80).AlignRight().Text($"$ {venta.Subtotal:F2}").FontSize(12);
                                });

                                totals.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);

                                totals.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text("IVA (12%):").Bold().FontSize(12);
                                    row.ConstantItem(80).AlignRight().Text($"$ {venta.Detalles.Sum(d => d.TotalConIva - d.Total):F2}").FontSize(12);
                                });

                                totals.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);

                                totals.Item().Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text("Total:").Bold().FontSize(14);
                                    row.ConstantItem(80).AlignRight().Text($"$ {venta.Total:F2}").Bold().FontSize(14);
                                });
                            });

                        // Advertencia tributaria
                        col1.Item().AlignCenter().PaddingTop(20)
                            .Text("Esta nota de venta no tiene validez tributaria con el SRI en Ecuador.")
                            .Italic().FontSize(12).FontColor(Colors.Grey.Darken2);
                    });

                    // Pie de página
                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Página ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            });

            document.GeneratePdf("output.pdf");

            var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;
            return stream;
        }


    }
}
