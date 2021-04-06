import {SelectItem, Translation} from 'primeng/api';

export interface Constants {
    OperationSuccessMessage: string;
    ReferenceItemSuccessOperation;
    ServerErrorMessage: string;
}

export const constants: Constants = {
    OperationSuccessMessage: 'Операция выполнена успешно.',
    ReferenceItemSuccessOperation: 'Операция выполнена успешно. Осуществляется возврат к странице справочника...',
    ServerErrorMessage: 'Сервер не отвечает или недоступен. Пожалуйста, повторите попытку позже'
};

export const yesNoAllSelectItems: SelectItem[] = [
    {label: 'все', value: ''},
    {label: 'нет', value: 'false'},
    {label: 'да', value: 'true'}
];

export const translation: Translation = {
    clear: 'Сбросить',
    dayNames: ['Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота', 'Воскресенье'],
    dayNamesMin: ['пн', 'вт', 'ср', 'чт', 'пт', 'сб', 'вс'],
    dayNamesShort: ['пн', 'вт', 'ср', 'чт', 'пт', 'сб', 'вс'],
    monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
    monthNamesShort: ['янв.', 'февр.', 'март', 'апр.', 'май', 'июнь', 'июль', 'авг.', 'сент.', 'окт.', 'ноябрь', 'дек.'],
    today: 'Сегодня'
};