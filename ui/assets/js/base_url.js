
  // config baseurl
  var host = $(location).attr('host');
  var base_url = '';
  if (host == 'localhost') {
    var base_url = host + '/lambda-frontend/';
  } else {
    var base_url = host;
  }
