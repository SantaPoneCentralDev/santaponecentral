﻿using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class SurveyQuestionXref
    {
        public Guid SurveyId { get; set; }
        public Guid SurveyQuestionId { get; set; }
        public string SortOrder { get; set; }
        public bool IsAction { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
