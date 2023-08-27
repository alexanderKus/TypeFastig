﻿using System;
using Application.Scores.Common;
using Domain.Models.Dtos;
using MediatR;
using OneOf;

namespace Application.Scores.Queries;

public record UserBestAccuracyScore(int UserId)
    : IRequest<OneOf<ScoreDto?, ScoreError>>;
