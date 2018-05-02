'use strict'

var gulp = require('gulp');
var browserSync = require('browser-sync').create();
var sass = require('gulp-sass');
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin')
var rename = require('gulp-rename');
var runSequence = require('run-sequence');

gulp.paths = {
  dist: 'dist/',
  src: 'src/',
  vendors: 'dist/vendors/'
};

var paths = gulp.paths;

require('require-dir')('./gulp-tasks');

// Static Server + watching scss/html files
gulp.task('serve', ['sass'], function() {

  browserSync.init({
    server: ['./', './src']
  });

  gulp.watch(paths.src + 'scss/**/*.scss', ['sass']);
  gulp.watch(paths.src + '**/*.html').on('change', browserSync.reload);
  gulp.watch(paths.src + 'js/**/*.js').on('change', browserSync.reload);

});

// Static Server without watching scss files
gulp.task('serve:lite', function() {

  browserSync.init({
    server: ['./', './src']
  });

  gulp.watch(paths.src + '**/*.css').on('change', browserSync.reload);
  gulp.watch(paths.src + '**/*.html').on('change', browserSync.reload);
  gulp.watch(paths.src + 'js/**/*.js').on('change', browserSync.reload);

});

gulp.task('serve:dist', function() {
  browserSync.init({
    server: ['./dist']
  });
});

gulp.task('sass', ['compile-vendors'], function() {
  return gulp.src(paths.src + '/scss/style.scss')
  .pipe(sass())
  .pipe(autoprefixer())
  .pipe(gulp.dest(paths.src + 'css'))
  .pipe(cssmin())
  .pipe(rename({suffix: '.min'}))
  .pipe(gulp.dest(paths.src + 'css'))
  .pipe(browserSync.stream());
});

gulp.task('sass:watch', function() {
  gulp.watch(paths.src + 'scss/**/*.scss', ['sass']);
});

gulp.task('default', ['serve']);


var useref = require('gulp-useref');
var gulpif = require('gulp-if');
var uglify = require('gulp-uglify');
var minifyCss = require('gulp-clean-css');
var del = require('del');

gulp.task('buildapp', ['clean', 'copyhtml', 'copyjs', 'copyimages', 'copycss'], function () {
    return gulp.src(paths.src + 'index.html')
        .pipe(useref())
        .pipe(gulpif(paths.src + 'js/**/*.js', uglify()))
        //.pipe(gulpif(paths.src + '**/*.css', minifycss()))
        .pipe(gulp.dest('dist'));
});

//gulp.task('fonts1', function() {
//  return gulp.src([
//          paths.src+'js/plugins/font-awesome/css/fontawesome-webfont.*'])
//          .pipe(gulp.dest('dist/css/'));
//});
//gulp.task('fonts2', function() {
//  return gulp.src([
//          paths.src+'js/plugins/simple-line-icons/css/Simple-Line-Icons.*'])
//          .pipe(gulp.dest('dist/css/'));
//});


gulp.task('copyhtml', function () {
    return gulp.src([paths.src + 'views/**/*', paths.dist+'views/**/*'])
     .pipe(gulp.dest('dist/views'));
      
});

gulp.task('clean', function () {
    return del('dist/**', { force: true });
});

gulp.task('copyjs', function () {
    return gulp.src([paths.src + 'js/**/*', paths.dist + 'js/**/*'])
     .pipe(gulp.dest('dist/js'));

});
gulp.task('copyimages', function () {
    return gulp.src([paths.src + 'img/**/*', paths.dist + 'img/**/*'])
     .pipe(gulp.dest('dist/img'));

});
gulp.task('copycss', function () {
    return gulp.src([paths.src + 'css/**/*', paths.dist + 'css/**/*'])
     .pipe(gulp.dest('dist/css'));

});
//gulp.task('scss', function () {
//    return gulp.src([paths.src + 'scss/**/*', paths.dist + 'scss/**/*'])
//     .pipe(gulp.dest('dist/scss'));

//});