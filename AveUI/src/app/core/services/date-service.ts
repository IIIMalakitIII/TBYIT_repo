import { Injectable } from '@angular/core';

import { MinLengthValidator } from '@angular/forms';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class DateService {

  constructor() { }
  isDateString(input: string) {
    return moment(input, moment.ISO_8601, true).isValid();
  }

  thisWeek(): Date {
    let weekNumber: number = this.weekNumber(new Date());
    return this.weekToDate(weekNumber, new Date().getFullYear());
  }

  nextWeek(): Date {
    let weekNumber: number = this.weekNumber(new Date()) + 1;
    let year = new Date().getFullYear();
    if (weekNumber > 52) {
      weekNumber = 1;
      year++;
    }
    return this.weekToDate(weekNumber, year);
  }

  lastWeek(): Date {
    let weekNumber: number = this.weekNumber(new Date()) - 1;
    let year = new Date().getFullYear();
    if (weekNumber < 1) {
      weekNumber = 52;
      year--;
    }
    return this.weekToDate(weekNumber, year);
  }

  thisMonth(): Date {
    let date: Date = new Date(),
      m: number = date.getMonth(),
      y: number = date.getFullYear();

    return new Date(y, m, 1);
  }

  lastMonth(): Date {
    let date: Date = new Date(),
      m: number = date.getMonth() - 1,
      y: number = date.getFullYear();

    if (m == -1) {
      m = 0;
      y--;
    }

    return new Date(y, m, 1);
  }

  thisYear(): Date {
    let y = new Date().getFullYear();
    return new Date(y, 0, 1);
  }

  tomorrow(): Date {
    let today = this.today();
    return this.addDays(today, 1);
  }

  today(): Date {
    let d: Date = moment().toDate();
    return new Date(d.getFullYear(), d.getMonth(), d.getDate());
  }

  now(): Date {
    return moment().toDate();
  }

  firstDayOfLastYear(): Date {
    let d = new Date(new Date().getFullYear() - 1, 0, 1);
    return d;
  }

  firstDayOfLastMonth(): Date {
    let d = new Date();
    d.setDate(1);
    d.setMonth(d.getMonth() - 1);
    return d;
  }

  firstDayOfCurrentMonth(): Date {
    const date = new Date();
    const y = date.getFullYear();
    const m = date.getMonth();
    const firstDay = new Date(y, m, 1);

    return firstDay;
  }

  firstDayOfThisYear(): Date {
    let d = new Date();
    d.setMonth(1);
    return d;
  }

  lastDayOfCurrentMonth(): Date {
    let d = new Date();
    return new Date(d.getFullYear(), d.getMonth() + 1, 0);
  }

  weekNumber(date: Date): number {
    let firstJan = new Date(date.getFullYear(), 0, 1);
    return Math.ceil((((date.getTime() - firstJan.getTime()) / 86400000) + firstJan.getDay() + 1) / 7);
  }

  weekToDate(weekNumber: number, year: number): Date {
    let d = (+ (weekNumber - 1) * 7);
    return new Date(year, 0, d);
  }

  minutesBetween(d1: Date, d2: Date): number {
    return this.hoursBetween(d1, d2) * 60;
  }

  hoursBetween(d1: Date, d2: Date): number {
    return Math.abs(d1.getTime() - d2.getTime()) / 36e5;
  }

  hoursBetweenWithSign(d1: Date, d2: Date): number {
    return (d1.getTime() - d2.getTime()) / 36e5;
  }

  secondsBetween(d1: Date, d2: Date) {
    return Math.abs((d1.getTime() / 1000) - (d2.getTime() / 1000));
  }

  prettyFormatRelativeToNow(date: Date, convertToUtc: boolean = true) {

    if (convertToUtc) {
      date = new Date(this.utcToLocal(date.toString()));
    }

    const time = date.getTime();
    const nowDate = new Date();
    const now = nowDate.getTime();

    if (isNaN(time)) {
      return '';
    }

    let millisec_diff = 0;

    if (time < now) {
      millisec_diff = now - time;
    } else {
      millisec_diff = time - now;
    }

    const date_diff = new Date(millisec_diff);
    const postfix = time > now ? 'from now' : 'ago';

    var diff = millisec_diff;
    var sign = diff < 0 ? -1 : 1;
    var milliseconds;
    var seconds;
    var minutes;
    var hours;
    var days;
    diff /= sign; // or diff=Math.abs(diff);
    diff = (diff - (milliseconds = diff % 1000)) / 1000;
    diff = (diff - (seconds = diff % 60)) / 60;
    diff = (diff - (minutes = diff % 60)) / 60;
    days = (diff - (hours = diff % 24)) / 24;

    let retVal = '';

    if (Math.abs(days) > 0) {
      retVal += days + ' days';
    }

    // const hours = date_diff.getHours();
    if (Math.abs(hours) > 0) {
      retVal += ' ' + hours + ' hours';
    }

    // const minutes = date_diff.getMinutes();
    if (Math.abs(minutes) > 0) {
      retVal += ' ' + minutes + ' minutes';
    }

    if (retVal.length == 0) {
      return 'Just now';
    }

    return retVal + ' ' + postfix;

    // return days + " days " + date_diff.getHours() + " hours " + date_diff.getMinutes()
    // + " minutes " + date_diff.getSeconds() + " seconds" + " " + postfix;
  }

  public utcToLocal(dateString: string): string {
    const utcTime = moment.utc(dateString);
    const offset = moment().utcOffset();
    const local = moment.utc(utcTime).utcOffset(offset).toISOString(true);

    if (local === null) { return null; }

    const parts = local.split('.');

    const val = parts[0].replace('T', ' ') + parts[1].substr(3, parts[1].length - 3);

    return val;
  }

  public localToUtc(dateString: string): string {
    let val = moment(dateString).utc().toString();
    const parts = val.split('.');
    return val;
  }

  format(d: Date): string {
    let out = moment(d).utc().format("YYYY-MM-DD HH:mm");
    return out;
  }

  formatDateOnly(d: Date): string {
    let out = moment(d).utc().format("YYYY-MM-DD");
    return out;
  }

  formatUtc(d: Date): string {
    let out = moment(d).format("YYYY-MM-DD HH;mm");
    return out;
  }

  addDays(date: Date, days: number): Date {
    date.setDate(date.getDate() + days);
    return date;
  }

  addMinutes(date: Date, minutes: number): Date {
    return moment(date).add(minutes, 'm').toDate();
  }

  public utcNow(keepOffset: boolean = false): any {
    return moment().utc().toISOString(keepOffset);
  }

  utcNowDate(keepOffset: boolean = false): Date {
    return new Date(moment().utc().toISOString(false).substr(0, 19));
  }

}
