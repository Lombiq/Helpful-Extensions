const recommendedSetup = require('../../Utilities/Lombiq.Gulp.Extensions/recommended-setup');

recommendedSetup.setupRecommendedScssAndJsTasks();

const gulp = require('gulp');
const jsTargets = require('../../Utilities/Lombiq.Gulp.Extensions/Tasks/js-targets');

const source = './Extensions/TargetBlank/Scripts/'
const destination = './wwwroot/TargetBlank'

gulp.task('build:scripts', () => jsTargets.compile(source, destination));
