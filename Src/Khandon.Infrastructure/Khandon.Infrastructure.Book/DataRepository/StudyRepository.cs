using Khandon.Core.Interfaces.IRepository;
using Khandon.Domain.Enitties.Book;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Shared.Dto.CompositionDtos;
using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Dto.Result;
using Khandon.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Infrastructure.Book.DataRepository
{
    public class StudyRepository : IStudyRepository
    {
        private readonly BookContext _bookContext;

        public StudyRepository(BookContext bookContext)
        {
            _bookContext = bookContext;
        }

        public async Task<StudyResult> GetStudyResultById(Guid Id, string UserId)
        {
            return await _bookContext.Studies.Include(a => a.Chapter.Book).Where(a => a.Chapter.Book.UserId == UserId && a.IsDeleted == false).Select(a => new StudyResult()
            {
                Id = a.Id,
                ChapterId = a.ChapterId,
                CreateDate = a.CreateDate,
                Length = a.Length,
                ShortDescription = a.ShortDescription,
                Title = a.Title,
                IsDeleted = a.IsDeleted,
                BookType = (Shared.Dto.Enums.BookTypeEnum)a.Chapter.Book.BookType,
                Flag=a.Flag
            }).FirstOrDefaultAsync(a => a.Id == Id && a.IsDeleted == false);
        }
        //ADD ONE STUDY
        public async Task<StudyResult> CreateStudyAsync(StudyCreate studyCreate, string userId)
        {
            if (studyCreate.Length <= 0)
            {
                throw new ArgumentException(message: "Length Is Invalid");
            }
            if (studyCreate == null)
            {
                throw new ArgumentNullException(paramName: "studyCreate");
            }
            if (string.IsNullOrEmpty(studyCreate.Title))
            {
                throw new ArgumentNullException(paramName: nameof(studyCreate.Title));
            }
            if (!_bookContext.Books.Include(a => a.Chapters).Any(a => a.Chapters.Any(c => c.Id == studyCreate.ChapterId) && a.UserId == userId))
            {
                throw new KeyNotFoundException("Chapter Id Invalid");
            }
            Study study = new Study()
            {
                Title = studyCreate.Title,
                ShortDescription = studyCreate.ShortDescription,
                ChapterId = studyCreate.ChapterId,
                CreateDate = MyDateTime.Now(),
                IsDeleted = false,
                Length = studyCreate.Length,
                Flag=studyCreate.Flag
            };
            try
            {
                await _bookContext.AddAsync(study);
                await _bookContext.SaveChangesAsync();
                return await GetStudyResultById(study.Id, userId);

            }
            catch (ArgumentNullException e)
            {
                throw e;
            }
        }

        //EDIT ONE STUDY
        public async Task<StudyResult> EditStudyAsync(StudyEdit studyEdit, string userId)
        {
            if (studyEdit.Length <= 0)
            {
                throw new ArgumentException(message: "Length Is Invalid");
            }
            if (studyEdit == null)
            {
                throw new ArgumentNullException(paramName: "studyEdit");
            }
            if (string.IsNullOrEmpty(studyEdit.Title))
            {
                throw new ArgumentNullException(paramName: nameof(studyEdit.Title));
            }
            if (_bookContext.Studies.Include(a => a.Chapter).ThenInclude(a => a.Book).FirstOrDefault(a => a.Id == studyEdit.Id
            && a.Chapter.Book.UserId == userId) is Study study)
            {
                study.Length = studyEdit.Length;
                study.Title = studyEdit.Title;
                study.ShortDescription = studyEdit.ShortDescription;
                study.Flag = studyEdit.Flag;

                try
                {
                    _bookContext.Update(study);
                    _bookContext.SaveChanges();
                    return await GetStudyResultById(study.Id, userId);

                }
                catch (ArgumentNullException e)
                {
                    throw e;
                }
            }
            throw new KeyNotFoundException("Study Not Found");

        }

        //SAFE DELETE STUDY
        public async Task<bool> SafeDeleteStudyAsync(string userId, Guid studyId)
        {
            if (_bookContext.Studies.Include(a => a.Chapter).ThenInclude(a => a.Book).FirstOrDefault(a => a.Id == studyId
            && a.Chapter.Book.UserId == userId) is Study study)
            {
                if (study.IsDeleted == false)
                {
                    study.IsDeleted = true;
                    _bookContext.SaveChanges();
                }
                return study.IsDeleted;
            }
            return false;
        }

        //GET STUDIES LIST WITH START/END DATE - SHOULD FILTER BY BOOK GROUP
        public async Task<List<StudyResult>> GetStudyListAsync(string userId, StudyGet studyGet)
        {
            if (studyGet.Limit >= 100)
                studyGet.Limit = 100;
            IQueryable<Study> studyQueryable = _bookContext.Studies.Include(a => a.Chapter).ThenInclude(a => a.Book).Where(a => a.Chapter.Book.UserId == userId && a.IsDeleted == false).AsQueryable();

            if (studyGet.Start != new DateTime())
            {
                studyQueryable = studyQueryable.Where(a => a.CreateDate >= studyGet.Start);
            }
            if (studyGet.End != new DateTime())
            {
                studyQueryable = studyQueryable.Where(a => a.CreateDate <= studyGet.End);
            }
            if (studyGet.BookGroupId != 0)
            {
                studyQueryable = studyQueryable.Where(a => a.Chapter.Book.BookGroupId == studyGet.BookGroupId);
            }

            studyQueryable = studyQueryable.Skip((studyGet.Page - 1) * studyGet.Limit).Take(studyGet.Limit);

            var studyResultQurable = await studyQueryable.Select<Study, StudyResult>(a => new StudyResult()
            {
                Id = a.Id,
                ChapterId = a.ChapterId,
                CreateDate = a.CreateDate,
                Length = a.Length,
                ShortDescription = a.ShortDescription,
                Title = a.Title,
                IsDeleted = a.IsDeleted,
                BookType = (Shared.Dto.Enums.BookTypeEnum)a.Chapter.Book.BookType,
                Flag=a.Flag
            }).ToListAsync();
            studyQueryable = null;
            return studyResultQurable;
        }

        public async Task<SocietyStatusDto> GetSocietyStatusAsync(string userId)
        {
            SocietyStatusDto societyStatusDto = new SocietyStatusDto();
            if (!_bookContext.Books.Any())
            {
                return societyStatusDto;
            }
            societyStatusDto.SumStudiesMinut = (int)_bookContext.Studies.Where(a => a.IsDeleted == false).Include(a => a.Chapter).ThenInclude(a => a.Book).Where(a => a.Chapter.Book.BookType == Domain.Enums.BookType.Minut).Sum(a => a.Length);

            societyStatusDto.SumStudiesPage = (int)_bookContext.Studies.Where(a => a.IsDeleted == false).Include(a => a.Chapter).ThenInclude(a => a.Book).Where(a => a.Chapter.Book.BookType == Domain.Enums.BookType.Page).Sum(a => a.Length);

            societyStatusDto.AllUsersCount = _bookContext.Books.Where(a => a.IsDeleted == false).GroupBy(a => a.UserId).Count();

            societyStatusDto.AvgDificalty = (int)_bookContext.Books.Where(a => a.IsDeleted == false).Average(a => a.Difficultye);

            //if (_bookContext.Books.Any(a => a.BookType == Domain.Enums.BookType.Minut))
            //{

            try
            {
                societyStatusDto.AvgStudyPerDayMinute = ((int)_bookContext.Studies.Where(a => a.IsDeleted == false).Include(a => a.Chapter).ThenInclude(a => a.Book).Where(a => a.Chapter.Book.BookType == Domain.Enums.BookType.Minut).Average(a => a.Length));
            }
            catch
            {
                societyStatusDto.AvgStudyPerDayMinute = 0;
            };
            //}
            //if (_bookContext.Books.Any(a => a.BookType == Domain.Enums.BookType.Page))
            //{
            try
            {
                societyStatusDto.AvgStudyPerDayPage = (int)_bookContext.Studies.Where(a => a.IsDeleted == false).Include(a => a.Chapter).ThenInclude(a => a.Book).Where(a => a.Chapter.Book.BookType == Domain.Enums.BookType.Page).Average(a => a.Length);
            }
            catch
            {
                societyStatusDto.AvgStudyPerDayPage = 0;
            }
            //}
            //UserRank=


            //-----Start---Calcute User Rank
            IDictionary<string, int> users = new Dictionary<string, int>();
            if (_bookContext.Books.Any(a => a.UserId == userId))
            {
                var books = _bookContext.Books.Where(a => a.IsDeleted == false).Include(a => a.Chapters).ThenInclude(a => a.Studies).Select(a => new UserRankRemainingCount()
                {
                    BookId = a.Id,
                    UserId = a.UserId,
                    StudyCount = a.Chapters.Select(a => a.Studies).Count()
                }).ToList().GroupBy(a => a.UserId);

                foreach (var item in books)
                {
                    var count = item.Select(a => a.StudyCount).Sum();
                    users.Add(item.Key, count);

                }
                societyStatusDto.UserRank = users.OrderByDescending(a => a.Value).ToList().IndexOf(users.First(a => a.Key == userId)) + 1;
            }
            //-----End---Calcute User Rank

            var bookGroups = _bookContext.Books.Where(a => a.IsDeleted == false).Include(a => a.BookGroup).Select(a => new BookGroupCompositionForGetActiveStatus()
            {
                Id = a.BookGroupId,
                Title = a.BookGroup.Title
            }).AsNoTracking().ToList();

            int countOfBooks = _bookContext.Books.Where(a => a.IsDeleted == false).Count();

            var bookGrouped = bookGroups.GroupBy(a => a.Id);

            societyStatusDto.ActiveStudyGroup = new Dictionary<string, int>();
            foreach (var item in bookGrouped)
            {
                int percentage = (item.Count() * 100) / countOfBooks;
                societyStatusDto.ActiveStudyGroup.Add(item.Select(a => a.Title).First(), percentage);
            }
            bookGroups = null;
            return societyStatusDto;
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

        private StatusActiveReadsDto FillLenthOfStatusActivationRead(StudyCompositionForGetPeriodReadTime item, StatusActiveReadsDto study)
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
        public async Task<List<StatusActiveReadsDto>> GetPeriodTimeSocityActivityAsync(ReadingActivityDateEnum time, int limit)
        {
            IEnumerable<StudyCompositionForGetPeriodReadTime> Studies = _bookContext.Books.Include(a => a.Chapters).ThenInclude(a => a.Studies).SelectMany(book => book.Chapters.SelectMany(chap => chap.Studies)).Select(a => new StudyCompositionForGetPeriodReadTime()
            {
                BookType = (BookTypeEnum)a.Chapter.Book.BookType,
                CreateDate = a.CreateDate,
                Length = a.Length
            }).AsNoTracking().OrderByDescending(a => a.CreateDate).ToList();

            switch (time)
            {
                case ReadingActivityDateEnum.DAY:
                    List<StatusActiveReadsDto> dayes = new();

                    foreach (var item in Studies)
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

                    foreach (var item in Studies)
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

                    foreach (var item in Studies)
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



                default:
                    return null;

            }
        }
    }
}