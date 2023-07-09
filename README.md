# CV
This is CV website Template for ATS.

I created this documentation for Tarek.

In your Create.cshtml view, you can take inspiration from this code for live CV writing update and also a download option:

@model ATSWebAppV2.Models.CV

@{
    ViewData["Title"] = "Create";
}

<h1>Create</h1>

<div id="cvContainer">
    <h4>CV</h4>
    <hr />
    <div class="row">
        <div class="col-md-4">
            <form asp-action="Create">
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                <div class="form-group">
                    <label asp-for="FullName" class="control-label"></label>
                    <input asp-for="FullName" class="form-control" id="fullName" />
                    <span asp-validation-for="FullName" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="Email" class="control-label"></label>
                    <input asp-for="Email" class="form-control" id="email" />
                    <span asp-validation-for="Email" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="Phone" class="control-label"></label>
                    <input asp-for="Phone" class="form-control" id="phoneNumber" />
                    <span asp-validation-for="Phone" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="Address" class="control-label"></label>
                    <input asp-for="Address" class="form-control" id="address" />
                    <span asp-validation-for="Address" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="Summary" class="control-label"></label>
                    <input asp-for="Summary" class="form-control" id="summary" />
                    <span asp-validation-for="Summary" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="WorkExperience" class="control-label"></label>
                    <input asp-for="WorkExperience" class="form-control" id="workExperience" />
                    <span asp-validation-for="WorkExperience" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="Education" class="control-label"></label>
                    <input asp-for="Education" class="form-control" id="education" />
                    <span asp-validation-for="Education" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label asp-for="HobbiesAndInterests" class="control-label"></label>
                    <input asp-for="HobbiesAndInterests" class="form-control" id="hobbies" />
                    <span asp-validation-for="HobbiesAndInterests" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <input type="submit" value="Create" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>
</div>

<div id="cvStyled">
    <div id="downloadablePDF">
        <style>
            body {
                font-family: Arial, sans-serif;
            }

            #downloadablePDF {
                margin: 0 auto;
                width: 600px;
                padding: 20px;
                background-color: #f2f2f2;
                border: 1px solid #ddd;
            }

            #nameDisplay {
                font-size: 24px;
                font-weight: bold;
                text-align: center;
                margin-bottom: 10px;
            }

            #contactDetails {
                text-align: center;
                margin-bottom: 20px;
            }

            #emailDisplay,
            #phoneNumberDisplay,
            #addressDisplay {
                font-size: 14px;
                display: inline-block;
                margin: 0 10px;
            }

            .section {
                margin-top: 20px;
                margin-bottom: 10px;
            }

            .section-title {
                font-size: 18px;
                font-weight: bold;
                margin-bottom: 5px;
            }
        </style>
        <h1 id="nameDisplay" style=""></h1>
        <div id="contactDetails">
            <h5 id="emailDisplay" style=""></h5>
            <h5 id="phoneNumberDisplay" style=""></h5>
            <h5 id="addressDisplay" style=""></h5>
        </div>
        <div class="section">
            <h3 class="section-title">Summary</h3>
            <p id="summaryDisplay" style=""></p>
        </div>

        <div class="section">
            <h3 class="section-title">Work Experience</h3>
            <ul id="workExperienceDisplay" style="">
                <li>[Work Experience 1]</li>
                <li>[Work Experience 2]</li>
                <li>[Work Experience 3]</li>
            </ul>
        </div>

        <div class="section">
            <h3 class="section-title">Education</h3>
            <ul id="educationDisplay" style="">
                <li>[Education 1]</li>
                <li>[Education 2]</li>
                <li>[Education 3]</li>
            </ul>
        </div>

        <div class="section">
            <h3 class="section-title">Hobbies</h3>
            <ul id="hobbiesDisplay" style="">
                <li>[Hobby 1]</li>
                <li>[Hobby 2]</li>
                <li>[Hobby 3]</li>
            </ul>
        </div>
    </div>
    <div>
        <a asp-action="Index">Back to List</a>
        <button id="downloadPdf" onclick="downloadPdf()">Download CV as PDF</button>
    </div>
</div>



@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}

    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.9.2/html2pdf.bundle.min.js"></script>

    <script>

        function downloadPdf() {
            console.log("Download CV as PDF clicked!"); // Added print statement
            var element = document.getElementById('downloadablePDF');
        html2pdf()
        .from(element)
        .save('cv.pdf');
        }

        $(document).ready(function () {
            $('#fullName').on('input', function () {
                var name = $(this).val();
                $('#nameDisplay').text(name);
            });

            $('#email').on('input', function () {
                var email = $(this).val();
                $('#emailDisplay').text(email);
            });

            $('#phoneNumber').on('input', function () {
                var phoneNumber = $(this).val();
                $('#phoneNumberDisplay').text(phoneNumber);
            });

            $('#address').on('input', function () {
                var address = $(this).val();
                $('#addressDisplay').text(address);
            });

            $('#summary').on('input', function () {
                var summary = $(this).val();
                $('#summaryDisplay').text(summary);
            });

            $('#workExperience').on('input', function () {
                var workExperience = $(this).val();
                $('#workExperienceDisplay').text(workExperience);
            });

            $('#education').on('input', function () {
                var education = $(this).val();
                $('#educationDisplay').text(education);
            });

            $('#hobbies').on('input', function () {
                var hobbies = $(this).val();
                $('#hobbiesDisplay').text(hobbies);
            });


        });</script>

}

Additionally, the model should be similar to the following:

using System;
namespace ATSWebAppV2.Models
{
	public class CV
	{
		public int id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Summary { get; set; }
        public string WorkExperience { get; set; }
        public string Education { get; set; }
        public string HobbiesAndInterests { get; set; }
    }
    
}

Let me know if you have any questions or need help!

Thank you,
Abdelrahman 
