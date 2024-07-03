using LovePdf.Core;
using LovePdf.Model.Task;
using LovePdf.Model.TaskParams.Sign.Elements;
using LovePdf.Model.TaskParams;
using System;
using LovePdf.Model.TaskParams.Sign;
using LovePdf.Model.Enums;
using System.Threading.Tasks;
using LovePdf.Model.TaskParams.Edit;

var api = new LovePdfApi("", "");

EditAdvanced();
//HTMLToPDFAdvanced();
//HtmlToPdf();
//EditPdf();
//CompressTest();
//await SignAdvanced();
//await SignManagment();


async Task SignBasic() {
    // Create sign task
    var task = api.CreateTask<SignTask>();

    // File variable contains server file name
    var file = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\3213961_3-2024.pdf");

    // Create task params
    var signParams = new SignParams();

    // Create a signer
    var signer = signParams.AddSigner("Franco", "franco.bdsso@gmail.com");

    // Add file that a receiver of type signer needs to sign.
    var signerFile = signer.AddFile(file.ServerFileName);

    // Add signers and their elements;
    var signatureElement = signerFile.AddSignature();
    signatureElement.Position = new Position(20, -20);
    signatureElement.Pages = "1";
    signatureElement.Size = 40;

    // Lastly send the signature request
    var signature = await task.RequestSignatureAsync(signParams);

    Console.WriteLine(signature.Status);
}

async Task SignManagment() {

    // Create sign management task
    var request = api.CreateTask<SignTask>();

    string signatureToken = "14f4d22acd5e640233aec33e86c324ab_TkUGw1dbc4a84393d3c19c069d277e8b49c40";
    string receiverToken = "745d781d0be1dab55c1729bcbe3d4d56_GWHKzc8e3fecaff84df769d236fe43d9793d1";

    // Get a list of all created signature requests
    var signaturesList = await request.GetSignaturesAsync();

    // Get the first page, with max number of 50 entries
    // per page (default is 20, max is 100).
    //var signaturesListFirstPage =
    //    await request.GetSignaturesAsync(new ListRequest(0, 50));

    // Get the current status of the signature:
    var signatureStatus =
        await request.GetSignatureStatusAsync(signatureToken);

    // Get information about a specific receiver:
    var receiverInfo =
        await request.GetReceiverInfoAsync(receiverToken);
    try
    {
        // Download the audit file on the filesystem
        //var auditFilePath =
        //    await request.DownloadAuditFileAsync(signatureToken, "C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions");
    }
    catch (Exception)
    {

    }


    //Download the original files on the filesystem:
    //It downloads a PDF file if a single file was uploaded. 
    // Otherwise a zip file with all uploaded files is downloaded.
    //var originalFilesPath =
    //    await request.DownloadOriginalFilesAsync(signatureToken, "C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\3213961_3-2024.pdf");

    // Download the created signed files on the filesystem:
    // It downloads a PDF file if a single file was uploaded. 
    // Otherwise a zip file with all uploaded files is downloaded.
    //var signedFilesPath =
    //    await request.DownloadSignedFilesAsync(signatureToken, "C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions");

    //Correct the email address of a receiver in
    // the event that the email was delivered to an
    // invalid email address

    try
    {
        var fixReceiverEmailResult =
            await request.FixReceiverEmailAsync(receiverToken, "franco.bdsso@hotmail.com");
    }
    catch (Exception)
    {

    }

    try
    {
        // Correct the mobile number of a signer in the
        // event that the SMS was delivered to an invalid
        // mobile number
        var fixSignerPhoneResult =
            await request.FixSignerPhoneAsync(receiverToken, "34666666668");
    }
    catch (Exception)
    {

    }

    try
    {

        //This endpoint sends an email reminder to pending
        // receivers.It has a daily limit quota(check the
        // docs to know the daily quota)
        var sendRemindersResult =
            await request.SendRemindersAsync(signatureToken);
    }
    catch (Exception)
    {

    }

    try
    {
        // Increase the number of days to '4' in order to
        // prevent the request from expiring and give receivers
        // extra time to perform remaining actions.
        var increaseExpirationDaysResult =
            await request.IncreaseExpirationDaysAsync(signatureToken, 4);
    }
    catch (Exception)
    {

    }

    try
    {
        // Void a signature that is currently in progress
        var voidSignatureResult =
            await request.VoidSignatureAsync(signatureToken);
    }
    catch (Exception)
    {

    }


    var signatureStatus2 =
        await request.GetSignatureStatusAsync(signatureToken);
}

async Task SignAdvanced()
{
    // Create sign task
    var task = api.CreateTask<SignTask>();

    // We first upload the files that we are going to use
    var file = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\3213961_3-2024.pdf");

    // Create task params
    var signParams = new SignParams();

    ////Set the Signature settings
    signParams.SubjectSigner = "Test subject";
    signParams.MessageSigner = "TestBody of the first message";

    signParams.SignerReminderDaysCycle = 3;
    signParams.SignerReminders = true;
    signParams.ExpirationDays = 130;

    signParams.Language = Languages.English;
    signParams.VerifyEnabled = true;
    signParams.LockOrder = false;
    signParams.UuidVisible = true;

    // Set brand
    //var logo = task.AddFile("C:\\Users\\franc\\Downloads\\Gmail2020New.png");
    //signParams.SetBrand("Test brand name", "C:\\Users\\franc\\Downloads\\Gmail2020New.png");

    ///////////////
    // RECEIVERS //
    ///////////////
    // Create the receivers
    //var validator = signParams.AddValidator("Validator", "franco.bdsso@gmail.com");
    //var viewer = signParams.AddViewer("Witness", "franco.bdsso@gmail.com");
    var signer = signParams.AddSigner("Franco", "franco.bdsso@gmail.com");

    //////////////
    // ELEMENTS //
    //////////////

    // Add file that a receiver of type signer needs to sign.
    var signerFile = signer.AddFile(file.ServerFileName);

    // Add elements to the receivers that need it
    //
    // "Pages" define rules:
    // - we can define the pages with a comma, e.g. "1,2"
    // - ranges can also be defined, e.g. "1-3"
    // - you can define multiple ranges, e.g. "1,2,3-6"
    
    var signatureElement = signerFile.AddSignature();
    signatureElement.Position = new Position(20, -20);
    signatureElement.Pages = "1";
    signatureElement.Size = 40;

    var dateElement = signerFile.AddDate("21/05/2024");
    dateElement.Position = new Position(20, -10);
    dateElement.Pages = "1";

    var initialsElement = signerFile.AddInitials();
    initialsElement.Position = new Position(20, -30);
    initialsElement.Pages = "1";

    var inputElement = signerFile.AddInput();
    inputElement.Position = new Position(50, -50);
    inputElement.Label = "Passport Number";
    inputElement.Description = "123456789";
    inputElement.Pages = "1";

    var nameElement = signerFile.AddName();
    nameElement.Position = new Position(60, -60);
    nameElement.Size = 40;
    nameElement.Pages = "1";

    var textElement = signerFile.AddText("This is a text field");
    textElement.Position = new Position(70, -70);
    textElement.Size = 40;
    textElement.Pages = "1";


    // Lastly send the signature request
    var signature = await task.RequestSignatureAsync(signParams);
}

void CompressTest()
{
    var task = api.CreateTask<CompressTask>();

    //add file, and specify rotation
    var file = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\3213961_3-2024.pdf", task.TaskId, Rotate.Degrees90);

    //set compress parameters and process files
    var time = task.Process(new CompressParams
    {
        //CompressionLevel = CompressionLevels.SuperLow,
        OutputFileName = "low_compression"
    });

    //download output file(s) to specific directory
    task.DownloadFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions");
}

void HtmlToPdf()
{
    //create protect task
    var task = api.CreateTask<HtmlToPdfTask>();

    //file variable contains server file name
    var file = task.AddFile(new Uri("https://ilovepdf.com"));

    //proces added files
    //time var will contains information about time spent in process
    var time = task.Process(new HTMLtoPDFParams());
    task.DownloadFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions");
}

void EditPdf()
{
    // Create edit task
    var task = api.CreateTask<EditTask>();

    // File variable contains server file name
    var file = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\3213961_3-2024.pdf");

    // Upload Image file to Ilovepdf servers
    var imageFile = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\Gmail2020New.png");

    // Create task params
    var editParams = new EditParams();

    // Create ImageElement
    var imageElement = editParams.AddImage(imageFile.ServerFileName);

    editParams.OutputFileName = "editpdf-basic";

    // Proces added files,
    // time var will contains information about time spent in process
    var time = task.Process(editParams);
    task.DownloadFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions");
}

void EditAdvanced()
{
    // Create edit task
    var task = api.CreateTask<EditTask>();

    // File variable contains server file name

    // Upload Image files to Ilovepdf servers
    var file = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\3213961_3-2024.pdf");
    var imageFile = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\Gmail2020New.png");
    var svgFile = task.AddFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions\\file_svg_icon_153410.svg");

    // Add elements to Editpdf task in order of drawing (important if elements overlap!)

    // Create edit task params
    var editParams = new EditParams();

    // Create ImageElement
    var imageElement = editParams.AddImage(imageFile.ServerFileName);
    imageElement.Coordinates = new Coordinate(300, 600);
    imageElement.Pages = 1;
    imageElement.Opacity = 40;

    // Create TextElement
    var textElement = editParams.AddText("This is a sample text");
    textElement.Coordinates = new Coordinate(300, 600);
    textElement.Pages = 1;
    textElement.Align = TextAligments.Center;
    textElement.FontFamily = FontFamilies.TimesNewRoman;
    textElement.FontColor = "#FB8B24"; // Orange
    textElement.FontStyle = TextFontStyles.Bold;

    // Create SvgElement
    var svgElement = editParams.AddSvg(svgFile.ServerFileName);
    svgElement.Coordinates = new Coordinate(300, 300);

    editParams.OutputFileName = "editpdf-advanced2";

    // Proces added files,
    // time var will contains information about time spent in process
    var time = task.Process(editParams);
    task.DownloadFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions");
}

void HTMLToPDFAdvanced()
{
    //create protect task
    var task = api.CreateTask<HtmlToPdfTask>();

    //file variable contains server file name
    var file = task.AddFile(new Uri("https://ilovepdf.com"));

    //proces added files
    //time var will contains information about time spent in process
    var time = task.Process(new HTMLtoPDFParams()
    {
        // set page margin
        Margin = 20,

        // set one large page
        SinglePage = true,

        // and set name for output file.
        // the task will set the correct file extension for you.
        OutputFileName = "ilovepdf_web"
    });
    task.DownloadFile("C:\\Users\\franc\\source\\repos\\ConsoleAppTestTGithubActions");
}
