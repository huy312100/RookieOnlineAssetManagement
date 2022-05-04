export const getFormatDateTime = (date) => {
  const DATE_OPTIONS = { day: "numeric", month: "numeric", year: "numeric" };
  return new Date(date).toLocaleDateString("vi-VN", DATE_OPTIONS);
};

export const convertDDMMYYStringToDate = (string) => {
  const date = string.split("/");
  var inputFormat = new Date(date[2], date[1] - 1, date[0]);
  function pad(s) { return (s < 10) ? '0' + s : s; }
  var d = new Date(inputFormat)
  return [pad(d.getDate()), pad(d.getMonth()+1), d.getFullYear()].join('/')
}