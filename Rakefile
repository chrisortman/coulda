require 'rake'

task :copy_xunit do

  files = FileList["Vendor/**/bin/Debug/*.dll", "Vendor/**/bin/Debug/*.pdb"]
  files.each { |f| cp f, "lib/xunit-1.5" }
end