using MauiDesktopApp.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace MauiDesktopApp.Services;

public class PdfService
{
    private readonly LogService _logService;

    public PdfService(LogService logService)
    {
        _logService = logService;
    }

    public async Task<string?> ExportSalarieAsync(Salarie salarie)
    {
        try
        {
            var exportDir = Path.Combine(FileSystem.AppDataDirectory, "exports");
            Directory.CreateDirectory(exportDir);

            var safeName = $"{salarie.Nom}_{salarie.Prenom}".Replace(' ', '_');
            var filePath = Path.Combine(exportDir, $"fiche_{safeName}_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var titleFont = new XFont("Arial", 16, XFontStyle.Bold);
            var textFont = new XFont("Arial", 12, XFontStyle.Regular);

            double y = 40;
            gfx.DrawString("Fiche salarié", titleFont, XBrushes.Black, new XRect(40, y, page.Width, 30));
            y += 40;

            void DrawLine(string label, string value)
            {
                gfx.DrawString($"{label}: {value}", textFont, XBrushes.Black, new XRect(40, y, page.Width - 80, 20));
                y += 24;
            }

            DrawLine("Nom", salarie.Nom);
            DrawLine("Prénom", salarie.Prenom);
            DrawLine("Téléphone fixe", salarie.TelephoneFixe);
            DrawLine("Téléphone portable", salarie.TelephonePortable);
            DrawLine("Email", salarie.Email);
            DrawLine("Service", salarie.Service?.Nom ?? string.Empty);
            DrawLine("Site", salarie.Site?.Nom ?? string.Empty);

            await using var stream = File.Create(filePath);
            document.Save(stream, false);

            return filePath;
        }
        catch (Exception ex)
        {
            await _logService.WriteErrorAsync($"Erreur export PDF: {ex.Message}");
            return null;
        }
    }
}
