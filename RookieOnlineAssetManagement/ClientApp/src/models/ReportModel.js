export default class ReportModel {
  constructor(
    category,
    total,
    assigned,
    available,
    notAvailable,
    waitingForRecycling,
    recycled
  ) {
    this.category = category;
    this.total = total;
    this.assigned = assigned;
    this.available = available;
    this.notAvailable = notAvailable;
    this.waitingForRecycling = waitingForRecycling;
    this.recycled = recycled;
  }
}
