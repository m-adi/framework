﻿
import * as React from 'react'
import { Dic } from '../Globals'
import { FindOptionsParsed } from '../FindOptions'
import { ValidationMessage, External } from '../Signum.Entities'
import { TypeReference } from '../Reflection'

export default class GroupByMessage extends React.Component<{ findOptions: FindOptionsParsed, mainType: TypeReference }>{
  render() {
    const fo = this.props.findOptions;

    const tokensObj = fo.columnOptions.map(a => a.token)
      .concat(fo.orderOptions.map(a => a.token))
      .filter(a => a != undefined && a.queryTokenType != "Aggregate")
      .toObjectDistinct(a => a!.fullKey, a => a!);

    const tokens = Dic.getValues(tokensObj);

    const message = ValidationMessage.TheRowsAreBeingGroupedBy0.niceToString().formatHtml(
      tokens.map(a => <strong>{a.niceName}</strong>).joinCommaHtml(External.CollectionMessage.And.niceToString()));
    return (
      <div className="sf-search-message alert alert-info">
        {"Ʃ"}&nbsp;{message}
      </div>
    );
  }

}



