using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Dto.Result;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Helper;
using Khandon.SharerdKernel.UI.Models;
using Khandon.SharerdKernel.UI.UtilitesService;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.State
{

    public delegate void BookChanges();
    public class BookStateService
    {
        private List<BookResult> Books = new List<BookResult>();
        private readonly IBookHttpService bookHttpService;
        private readonly IChapterHttpService chapterHttpService;
        private readonly IStudyHttpService studyHttpService;
        private readonly ISnackbar snackbar;

        public event BookChanges OnNewBookAdded; // event

        public BookStateService(IBookHttpService bookHttpService, ISnackbar snackbar, IChapterHttpService chapterHttpService, IStudyHttpService studyHttpService)
        {
            this.bookHttpService = bookHttpService;
            this.snackbar = snackbar;
            this.chapterHttpService = chapterHttpService;
            this.studyHttpService = studyHttpService;
        }
        public async Task<List<BookResult>> GetBooksAsync()
        {
            if (!Books.Any())
            {
                try
                {
                    Console.WriteLine("fetch 'BOOKS' from network");
                    var allbooks = await bookHttpService.GetAllBooksAsync();
                    if (allbooks.Books.Any())
                    {
                        Books = allbooks.Books;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //Books = new List<BookResult>();
                    snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
                }
            }
            return Books;
        }

        public async Task<BookResult> GetBookByIdAsync(Guid Id)
        {
            if (Books.Count == 0)
            {
                await GetBooksAsync();
            }
            var _book = Books.FirstOrDefault(a => a.Id == Id);
            if (_book != null)
            {
                return _book;
            }

            return null;
        }

        public async Task<BookResult> CreateChapterAsync(ChapterCreate chapterCreate)
        {
            try
            {
                var chapterResult = await chapterHttpService.CreateChapterAsync(chapterCreate);
                if (chapterResult != null)
                {
                    Books.Where(a => a.Id == chapterResult.BookId).FirstOrDefault().Chapters.Add(chapterResult);

                    return Books.Where(a => a.Id == chapterResult.BookId).First();
                }
                return null;
            }
            catch
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
                return null;
            }
        }

        public async Task<BookResult> CreateStudyAsync(StudyCreate studyCreate)
        {
            try
            {
                var studyResult = await studyHttpService.CreateStudyAsync(studyCreate);
                if (studyResult != null)
                {
                    var book = Books.Where(a => a.Chapters.Any(a => a.Id == studyResult.ChapterId)).FirstOrDefault();

                    var chapter = book.Chapters.First(a => a.Id == studyResult.ChapterId);

                    chapter.Studies.Add(studyResult);

                    return book;

                }
                return null;
            }
            catch (Exception e)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
                return null;
            }
        }

        public async Task CreateBookAsync(BookCreate bookCreate)
        {
            try
            {
                var bookResult = await bookHttpService.CreateBookAsync(bookCreate);
                if (bookResult != null)
                {
                    Books.Add(bookResult);
                    OnNewBookAdded();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
            }
        }

        public async Task<MyStatusDto> GetMyStatusAsync()
        {
            if (!Books.Any())
            {
                try
                {
                    var allbooks = await bookHttpService.GetAllBooksAsync();
                    if (allbooks.Books.Any())
                    {
                        Books = allbooks.Books;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //Books = new List<BookResult>();
                    snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
                }
            }

            MyStatusDto myStatus = new();
            if (Books.Any()&& Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies)).Any())
            {

                myStatus.DificaltyAvg = ((int)Books.Average(a => a.Difficultye));
                if (Books.Any(a => a.BookType == BookTypeEnum.Minut)&& Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies)).Any(a=> a.BookType==BookTypeEnum.Minut))
                {
                    myStatus.AvgReadingMinute = (int)Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies).Where(a => a.BookType == BookTypeEnum.Minut)).Average(a => a.Length);

                    myStatus.MaxReadingMinute = (int)Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies).Where(a => a.BookType == BookTypeEnum.Minut)).Max(a => a.Length);


                    myStatus.TotalReadingMinute = (int)Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies).Where(a => a.BookType == BookTypeEnum.Minut)).Sum(a => a.Length);

                }

                if (Books.Any(a => a.BookType == BookTypeEnum.Page)&& Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies)).Any(a => a.BookType == BookTypeEnum.Page))
                {
                    myStatus.AvgReadingPage = (int)Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies).Where(a => a.BookType == BookTypeEnum.Page)).Average(a => a.Length);

                    myStatus.MaxReadingPage = (int)Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies).Where(a => a.BookType == BookTypeEnum.Page)).Max(a => a.Length);

                    myStatus.TotalReadingPage = (int)Books.SelectMany(a => a.Chapters.SelectMany(b => b.Studies).Where(a => a.BookType == BookTypeEnum.Page)).Sum(a => a.Length);
                }


                myStatus.IsData = true;

                var groupedByBookGroup = Books.GroupBy(a => a.BookGroup.Id);

                int countOfBooks = Books.Count;
                myStatus.ActiveStudyGroup = new Dictionary<string, int>();
                foreach (var item in groupedByBookGroup)
                {
                    string bookGroup = Books.Where(a => a.BookGroupId == item.Key).FirstOrDefault().BookGroup.Title;
                    int percentage = (item.Count() * 100) / countOfBooks;

                    myStatus.ActiveStudyGroup.Add(bookGroup, percentage);
                }
            }
            else
            {
                myStatus.IsData = false;
            }
            return myStatus;
        }
        private List<StatusActiveReadsDto> GetDistincetMyStatusActiveReads(List<StatusActiveReadsDto> studies)
        {
            List<StatusActiveReadsDto> result = new();

            foreach (var study in studies)
            {
                result.Add(new()
                {
                    LengthMinut = 0,
                    LengthPage = 0,
                    Label = study.Label,
                    Data = study.Data
                });
                foreach (var orginal in studies)
                {
                    if (study.Label == orginal.Label)
                    {

                        result.FirstOrDefault(a => a.Label == orginal.Label).LengthPage += orginal.LengthPage;

                        result.FirstOrDefault(a => a.Label == orginal.Label).LengthMinut += orginal.LengthMinut;
                        orginal.LengthPage = 0;
                        orginal.LengthMinut = 0;
                        study.LengthMinut = 0;
                        study.LengthPage = 0;
                    }
                }
            }
            return result;
        }

        private StatusActiveReadsDto FillLenthOfStatusActivationRead(StudyResult item, StatusActiveReadsDto study)
        {

            if (item.BookType == BookTypeEnum.Minut)
            {
                study.LengthMinut = item.Length;
            }
            else
            {
                study.LengthPage = item.Length;
            }
            return study;
        }
        public async Task<List<StatusActiveReadsDto>> GetPeriodTimeActivityAsync(ReadingActivityDateEnum readingPeriod, int limit)
        {
            if (!Books.Any())
            {
                try
                {
                    var allbooks = await bookHttpService.GetAllBooksAsync();
                    if (allbooks.Books.Any())
                    {
                        Books = allbooks.Books;
                    }
                }
                catch (Exception e)
                {

                    snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
                }
            }
            if (Books.Any())
            {
                var studies = Books.SelectMany(a => a.Chapters).SelectMany(a => a.Studies.ToList()).OrderByDescending(a => a.CreateDate).ToList();

                switch (readingPeriod)
                {
                    //-----------DAY----------------
                    case ReadingActivityDateEnum.DAY:
                        List<StatusActiveReadsDto> dayes = new();

                        foreach (var item in studies)
                        {
                            var study = new StatusActiveReadsDto()
                            {
                                Data = item.CreateDate.ToShamsi(),
                                Label = item.CreateDate.ToShamsi().Substring(5),
                            };
                            dayes.Add(FillLenthOfStatusActivationRead(item, study));

                        }
                        List<StatusActiveReadsDto> resultListDayes = GetDistincetMyStatusActiveReads(dayes);
                        dayes = null;
                        return resultListDayes.Where(a => a.LengthMinut != 0 || a.LengthPage != 0).Take(limit).ToList();


                    //------------Week----------------
                    case ReadingActivityDateEnum.WEEK:
                        List<StatusActiveReadsDto> weeks = new();

                        foreach (var item in studies)
                        {
                            //Console.WriteLine(item.Title);

                            var pastOfToWeek = (DateTime.Today - item.CreateDate).Days / 7;

                            var study =
                            new StatusActiveReadsDto()
                            {
                                Data = ((pastOfToWeek == 0) ? "همین هفته" :
                                $"{pastOfToWeek} هفته پیش"),

                                Label = ((pastOfToWeek == 0) ? "این هفته" : $"هفته {pastOfToWeek}")
                            };
                            weeks.Add(FillLenthOfStatusActivationRead(item, study));

                        }

                        List<StatusActiveReadsDto> resultListWeek = GetDistincetMyStatusActiveReads(weeks);
                        weeks = null;
                        return resultListWeek.Where(a => a.LengthMinut != 0 || a.LengthPage != 0).Take(limit).ToList();


                    //-----------Month------------------
                    case ReadingActivityDateEnum.MONTH:

                        List<StatusActiveReadsDto> months = new();

                        List<string> persionMonth = new()
                        {
                            "فروردین",
                            "اردیبهشت",
                            "خرداد",
                            "تیر",
                            "مرداد",
                            "شهریور",
                            "مهر",
                            "آبان",
                            "اذر",
                            "دی",
                            "بهمن",
                            "اسفند",
                        };

                        foreach (var item in studies)
                        {
                            //Console.WriteLine(item.Title);

                            var pastOfToMonth = (DateTime.Today - item.CreateDate).Days / 30;
                            var persionCalander = new PersianCalendar();
                            var study = new StatusActiveReadsDto()
                            {
                                Data = persionMonth[persionCalander.GetMonth(item.CreateDate.Date) - 1],
                                Label = persionMonth[persionCalander.GetMonth(item.CreateDate.Date) - 1]
                            };
                            months.Add(FillLenthOfStatusActivationRead(item, study));

                        }

                        List<StatusActiveReadsDto> resultListMonth = GetDistincetMyStatusActiveReads(months);
                        months = null;

                        return resultListMonth.Where(a => a.LengthMinut != 0 || a.LengthPage != 0).Take(limit).ToList();

                }
            }

            return null;
        }


        public async Task<SocietyStatusDto> GetSocietyStatus()
        {
            try
            {
                SocietyStatusDto result = await studyHttpService.GetSocietyStatusAsync();
                if (result != null)
                    return result;
                return null;
            }
            catch (Exception)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);            
            }
            return null;
        }


        public async Task<List<StatusActiveReadsDto>> GetPeriodTimeSocityActivityAsync(
            int limit = 10, ReadingActivityDateEnum time = ReadingActivityDateEnum.WEEK)
        {
            try
            {
                List<StatusActiveReadsDto> result = await studyHttpService.GetPeriodTimeSocityActivityAsync(limit,time);
                if (result != null)
                    return result;
                return null;
            }
            catch (Exception)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
            }
            return null;
        }

        public async Task ClearState()
        {
            Books = new();
        }
    }
}
