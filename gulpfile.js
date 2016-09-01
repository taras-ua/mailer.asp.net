var gulp = require('gulp');
var argv = require('yargs').argv;
var cp = require('copyfiles');

/**
 * Common functions
 */

var copyBuild = function(destination, callback) {
  var pathArray = ['Mailer\\Views\\Shared\\_EmailLayout.cshtml',
                   'Mailer\\Views\\Email\\*.cshtml',
                   'Mailer\\Models\\EmailViewModels.cs',
                   'Mailer\\Controllers\\EmailController.cs',
                   destination];
  console.log('> Assembling build at ' + destination + '...'); 
  cp(pathArray, 1, callback);
};

var createLocalBuild = function(destination, callback) {
  copyBuild(destination, function() {
        console.log('> Done.'); 
        callback();
  });
}

var path = './dist';

gulp.task('define-path', function(cb) {
  if (argv.path || argv.p) {
    path = argv.path ? argv.path : argv.p;
  }
  cb();
});

/**
 * LOCAL BUILD TASKS
 * 
 * $ gulp build
 */

gulp.task('dist-local-build', ['define-path'], function(cb) {
  createLocalBuild(path, cb);
});

gulp.task('build', ['dist-local-build']);