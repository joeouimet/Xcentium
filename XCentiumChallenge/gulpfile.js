﻿var gulp = require('gulp');
var sass = require('gulp-sass');

gulp.task('sass', function ()
{
    gulp.src('./content/*.scss')
      .pipe(sass({ outputStyle: 'compressed' }))
      .pipe(gulp.dest('./content/'));
});