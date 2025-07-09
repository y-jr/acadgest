using acadgest.Dto.Mark;
using Azure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace acadgest.Views.Document
{
    public class Boletim : IDocument
    {
        public BoletimDto BoletimDto { get; set; } = new();

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            using var stream = new FileStream("wwwroot/img/fenda.jpg", FileMode.Open);
            container.Page(page =>
            {
                page.Size(PageSizes.A6);
                page.MarginVertical(10);
                page.MarginHorizontal(10);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                   .Column(column =>
                   {
                       column.Item().AlignCenter().Height(40)
                           .Image(stream); // Carrega a imagem
                       column.Item().AlignCenter()
                           .Text("COLÉGIO FENDA DA TUNDAVALA")
                           .FontSize(13)
                           .FontColor("#000")
                           .SemiBold();
                       column.Item().AlignCenter()
                           .Text("ENSINO POR EXCELÊNCIA")
                           .FontSize(8)
                           .FontColor("#000");
                   });

                page.Content()
                    .PaddingVertical(5)
                    .Column(Column =>
                    {
                        Column.Item().AlignCenter()
                        .Text("Boletim de notas do IIº Trimestre - 2024-2025")
                        .FontSize(11);
                        Column.Item().Height(10);
                        Column.Item().AlignLeft()
                        .Text($"{BoletimDto.ClassName}")
                        .FontSize(11);
                        Column.Item().AlignLeft().Background("#9dd6ff").Padding(4).Width(265)
                        .Text($"Aluno: {BoletimDto.PupilName}")
                        .FontSize(12);
                        Column.Item().Height(10);
                        Column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(30);
                                    columns.ConstantColumn(30);
                                    columns.ConstantColumn(30);
                                    columns.ConstantColumn(30);
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(30);
                                });

                                table.Cell().Text("Disciplina").FontSize(10);
                                table.Cell().Text("MAC").FontSize(10);
                                table.Cell().Text("PP").FontSize(10);
                                table.Cell().Text("PT").FontSize(10);
                                table.Cell().Text("MT").FontSize(10);
                                table.Cell().Text("Estado").FontSize(10);
                                table.Cell().Text("Obs").FontSize(10);
                                bool color = true;
                                float mg = 0;
                                int count = 0;
                                foreach (var mark in BoletimDto.Marks)
                                {
                                    mg += mark.Mt;
                                    count++;
                                    if (color)
                                    {
                                        color = false;
                                        table.Cell().Background("#cecece").Height(16).AlignMiddle().Text($"{mark.Subject}").FontSize(9);
                                        table.Cell().Background("#cecece").Height(16).AlignMiddle().Text($"{mark.Mac:0.0}").FontSize(8);
                                        table.Cell().Background("#cecece").Height(16).AlignMiddle().Text($"{mark.Pp:0.0}").FontSize(8);
                                        table.Cell().Background("#cecece").Height(16).AlignMiddle().Text($"{mark.Pt:0.0}").FontSize(8);
                                        table.Cell().Background("#cecece").Height(16).AlignMiddle().Text($"{mark.Mt:0.0}").FontSize(8);
                                        if (mark.Mt < 10)
                                        {
                                            table.Cell().Background("#cecece").Height(16).AlignMiddle().Text("Reprovado").FontSize(8).FontColor("#ff1111");
                                        }
                                        else
                                        {
                                            table.Cell().Background("#cecece").Height(16).AlignMiddle().Text("Aprovado").FontSize(8).FontColor("#111bff");
                                        }
                                        table.Cell().Background("#cecece").Height(16).AlignMiddle().Text("-").FontSize(8).AlignCenter();
                                    }
                                    else
                                    {
                                        color = true;
                                        table.Cell().Height(16).AlignMiddle().Text($"{mark.Subject}").FontSize(9);
                                        table.Cell().Height(16).AlignMiddle().Text($"{mark.Mac:0.0}").FontSize(8);
                                        table.Cell().Height(16).AlignMiddle().Text($"{mark.Pp:0.0}").FontSize(8);
                                        table.Cell().Height(16).AlignMiddle().Text($"{mark.Pt:0.0}").FontSize(8);
                                        table.Cell().Height(16).AlignMiddle().Text($"{mark.Mt:0.0}").FontSize(8);
                                        if (mark.Mt < 10)
                                        {
                                            table.Cell().Height(16).AlignMiddle().Text("Reprovado").FontSize(8).FontColor("#ff1111");
                                        }
                                        else
                                        {
                                            table.Cell().Height(16).AlignMiddle().Text("Aprovado").FontSize(8).FontColor("#111bff");
                                        }
                                        table.Cell().Height(16).AlignMiddle().Text("-").FontSize(8).AlignCenter();
                                    }
                                }
                                table.Cell().ColumnSpan(7).Height(16);
                                table.Cell().ColumnSpan(3).Background("#46f8ff").Height(16).AlignMiddle().Text("Média geral").FontSize(10).AlignLeft();
                                if ((mg / count) >= 15)
                                {
                                    table.Cell().ColumnSpan(2).Background("#2fff36").Height(16).AlignMiddle().Text($"{(mg / count):0.0}").FontSize(10).AlignCenter();
                                    table.Cell().Background("#2fff36").Height(16).AlignMiddle().Text($"{(((mg / count) / 20) * 100):0.0} %").FontSize(10).AlignCenter();
                                }
                                else if ((mg / count) >= 12)
                                {
                                    table.Cell().ColumnSpan(2).Background("#b8ff5b").Height(16).AlignMiddle().Text($"{(mg / count):0.0}").FontSize(10).AlignCenter();
                                    table.Cell().Background("#b8ff5b").Height(16).AlignMiddle().Text($"{(((mg / count) / 20) * 100):0.0} %").FontSize(10).AlignCenter();
                                }
                                else if ((mg / count) >= 10)
                                {
                                    table.Cell().ColumnSpan(2).Background("#f2ff5b").Height(16).AlignMiddle().Text($"{(mg / count):0.0}").FontSize(10).AlignCenter();
                                    table.Cell().Background("#f2ff5b").Height(16).AlignMiddle().Text($"{(((mg / count) / 20) * 100):0.0} %").FontSize(10).AlignCenter();
                                }
                                else if ((mg / count) >= 7)
                                {
                                    table.Cell().ColumnSpan(2).Background("#ff9834").Height(16).AlignMiddle().Text($"{(mg / count):0.0}").FontSize(10).AlignCenter();
                                    table.Cell().Background("#ff9834").Height(16).AlignMiddle().Text($"{(((mg / count) / 20) * 100):0.0} %").FontSize(10).AlignCenter();
                                }
                                else if ((mg / count) >= 4)
                                {
                                    table.Cell().ColumnSpan(2).Background("#ff0600").Height(16).AlignMiddle().Text($"{(mg / count):0.0}").FontSize(10).AlignCenter();
                                    table.Cell().Background("#ff0600").Height(16).AlignMiddle().Text($"{(((mg / count) / 20) * 100):0.0} %").FontSize(10).AlignCenter();
                                }
                                else
                                {
                                    table.Cell().ColumnSpan(2).Background("#000").Height(16).AlignMiddle().Text($"{(mg / count):0.0}").FontSize(10).FontColor("#ff0600").AlignCenter();
                                    table.Cell().Background("#000").Height(16).AlignMiddle().Text($"{(((mg / count) / 20) * 100):0.0} %").FontSize(10).FontColor("#ff0600").AlignCenter();
                                }
                            });
                    });

                page.Footer()
                    .AlignLeft()
                    .Text($"O Director de Turma: {BoletimDto.ClassDirectorName}")
                    .FontSize(10);

            });
        }
    }
}