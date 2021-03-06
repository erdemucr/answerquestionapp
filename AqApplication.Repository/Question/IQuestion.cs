﻿using AnswerQuestionApp.Repository.FilterModels;
using AqApplication.Core.Type;
using AqApplication.Entity.Challenge;
using AqApplication.Entity.Question;
using AqApplication.Repository.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Repository.Question
{
    public interface IQuestion
    {
        #region Question
        Result SaveQuestion(QuestionMain model);

        Result<IEnumerable<QuestionMain>> GetQuestion(QuestionFilterModel model);

        Result<IEnumerable<QuestionAnswer>> GetAnswers(int questionId);

        Result DeleteQuestion(int questionId);


        #endregion

        #region Lecture
        Result AddLecture(Lecture model, string userId);

        Result<IEnumerable<Lecture>> GetLectures();

        Result SetLectureStatus(int id, string userId);

        Result EditLecture(Lecture model, string userId);
        Result DeleteLecture(int id);
        Result<Lecture> GetLectureByKey(int id);

        Result<IEnumerable<Lecture>> GetLecturesByExamId(int examId);

        #endregion

        #region Subject
        Result AddSubject(Subject model, string userId);
        Result<IEnumerable<Subject>> GetSubjects();
        Result<Subject> GetSubjectByKey(int id);
        Result SetSubjectStatus(int id, string userId);
        Result EditSubject(Subject model, string userId);

        Result DeleteSubject(int id);

        Result<IEnumerable<Subject>> GetSubjectsByLectureId(int lectureId);

        #endregion

        #region SubSubject

        Result AddSubSubject(SubSubject model, string userId);

        Result<IEnumerable<SubSubject>> GetSubSubjects(int subjectId);

        Result<IEnumerable<SubSubject>> GetSubSubjectsAll();

        Result<SubSubject> GetSubSubjectByKey(int id);

        Result SetSubSubjectStatus(int id, string userId);

        Result EditSubSubject(SubSubject model, string userId);

        Result DeleteSubSubject(int id);

        #endregion

        #region Exam
        Result AddExam(Exam model, string userId);
        Result<IEnumerable<Exam>> GetExams();
        Result<Exam> GetExamByKey(int id);
        Result SetExamStatus(int id, string userId);
        Result EditExam(Exam model, string userId);
        Result DeleteExam(int id);
        #endregion


        #region Class
        Result AddClass(Class model, string userId);
        Result<IEnumerable<Class>> GetClass();
        Result<Class> GetClassByKey(int id);
        Result SetClassStatus(int id, string userId);
        Result EditClass(Class model, string userId);
        Result DeleteClass(int id);



        #endregion
        #region QuizTemplate
        Result<IEnumerable<ChallengeTemplate>> GetChallengeTemplates(ChallengeFilterModel model);

        Result AddChallengeTemplate(ChallengeTemplate model, string userId);

        Result<Entity.Challenge.ChallengeTemplate> GetChallengeTemplateByKey(int id);

        Result EditChallengeTemplate(ChallengeTemplate model, string userId);

        Result<IEnumerable<ChallengeTemplateItems>> GetChallengeTemplateItems(int id);

        Result AddChallengeTemplateItem(ChallengeTemplateItems model, string userId);

        Result EditChallengeTemplateItem(ChallengeTemplateItems model, string userId);

        Result<ChallengeTemplateItems> GetChallengeTemplateItemByKey(int id);

        Result UpdateOrdersChallengeTemplateItem(string idOrderMatch);
        #endregion


        #region Difficulty

        Result AddDifficulty(Difficulty model, string userId);
        Result<IEnumerable<Difficulty>> Difficulty();
        Result<Difficulty> GetDifficultyByKey(int id);
        Result SetDifficultyStatus(int id, string userId);
        Result EditDifficulty(Difficulty model, string userId);
        Result DeleteDifficulty(int id);

        #endregion

        Result SetQuestionCorrectAnswer(int questionId, string questionContent, int correctIndex, string userId);

    }
}